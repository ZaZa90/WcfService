using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

// Can be improved replacing Operation with string coded as Lddd where L is a letter and ddd are 3 digits describing the angle
// For ex. R127 will be rotation RIGHT by 127 degree 
public enum Operation { STOP, FORWARD, RIGHT, LEFT, PICTURE, NULL, ERROR, FORWARD2, BACKWARD2, RIGHT2, LEFT2 };

public enum Dir { NORTH, SOUTH, EAST, WEST };

namespace WcfService
{
    public class Database
    {
        static string ip = "0.0.0.0";
        static Picture picture;
        static int pictureId = 0;
        static Queue<String> operations = new Queue<String>();
        static Dictionary<String, String> products = new Dictionary<string, string>();
        //static Queue<string> operations = new Queue<string>();

        static string target = null;
        static string currPos = "A0";
        static List<string> checks;
        static String currentOp = "";
        static string currentDest;
        static string status = "";
        static bool dbLock = false;
        static int storageDim = 3; //Default value: 3
        static bool init = true;
        static float[] conf = { 0,0,0,0};
        static Dir currDir = Dir.NORTH;

        public void setConf(float v0=0, float v1=0, float v2=0, float v3=0) {
            if (v0 != 0) conf[0] = v0;
            if (v1 != 0) conf[1] = v1;
            if (v2 != 0) conf[2] = (float)Math.Round(v2);
            if (v3 != 0) conf[3] = v3;
        }

        public float getConf(int index) { return conf[index]; }

        public void setStorageDim(int val) { storageDim = val; }
        public int getStorageDim() { return storageDim; }

        public void setInit(bool b) { init = b; }
        public bool needsInit() { return init; }

        public Queue<String> getOperations() { return operations; }
        //        public Queue<string> getOperations() { return operations; }

        public void addOperation(String op) { operations.Enqueue(op); }
        //        public void addOperation(Operation op, int degree) { 
        //      if(degree <0 || degree>180) return;
        //      string oper;
        //      switch(op){
        //          case LEFT:
        //                         oper = 'L';
        //                        break;
        //          default:
        //                         oper = 'R';
        //                        break;
        //      }
        //      string command = oper + degree;
        //      operations.Enqueue(command); 
        //    }

        public String getOperation()
        {
            if (operations.Count > 0)
            {
                currentOp = readOperation();
                return operations.Dequeue();
            }
            else
            {
                currentOp = "";
                return "NULL";
            }
        }

        //        public string getOperation()
        //        {
        //            if (operations.Count > 0)
        //            {
        //                currentOp = readOperation();
        //                return operations.Dequeue();
        //            }
        //            else
        //                return NULL;
        //        }

        public String readOperation()
        {
            if (operations.Count > 0)
                return operations.ElementAt(0);
            else
                return "NULL";
        }

//        public string readOperation()
//        {
//            if (operations.Count > 0)
//                return operations.ElementAt(0);
//            else
//                return NULL;
//        }

        public void removeAllOperation()
        {
            while (operations.Count > 0)
            {
                operations.Dequeue();
            }
        }

        public string GetIp() { return ip; }
        public void SetIp(string val) { ip = val; }

        public Picture GetPicture() { return picture; }
        public void SetPicture(Picture val) { picture = val; }

        public string getStatus()
        {
            return status;
        }

        public String getCurrentOperation()
        {
            return currentOp;
        }

        public void setLock(bool val)
        {
            dbLock = val;
        }
        public bool getLock()
        {
            return dbLock;
        }

        public Dictionary<String, String> getProducts()
        {
            return products;
        }

        public void setPosition(String pos)
        {
            currPos = pos;
        }
        public String getPosition()
        {
            return currPos;
        }

        public void setDir(String dir)
        {
            switch (dir)
            {

                case "NORTH":
                    currDir = Dir.NORTH;
                    break;
                case "EAST":
                    currDir = Dir.EAST;
                    break;
                case "SOUTH":
                    currDir = Dir.SOUTH;
                    break;
                case "WEST":
                    currDir = Dir.WEST;
                    break;
            }
        }
        public string getDir()
        {
            return currDir.ToString();
        }

        public int NewPictureId() {
            pictureId++;
            return pictureId; 
        }
        
        public void SetTargetAndChecks(string tar, List<string> checkpoints)
        {
            target = tar;
            checks = new List<string>();
            foreach(var str in checkpoints)
                if(!str.Equals(tar)) checks.Add(str);
        }

        // If target is set return next target, otherwise calls getOperation()
        public String getNextLocalTarget()
        {
            int i;
            Dir dir;
            // TODO
            float angle;
            if (!picture.getQRCode().Equals("Error"))
            {
                currPos = picture.getQRCode();
                angle = picture.getAngle();
                dir = (angle > 45 && angle <= 135) ? Dir.EAST : (angle > 135 && angle <= 225) ? Dir.SOUTH : (angle > 225 && angle <= 315) ? Dir.WEST : Dir.NORTH;
                currDir = dir;
            }
            else
            {
                dir = currDir;
            }
            if (target != null)
            {
                if (currentDest == null || currPos.Equals(currentDest))
                {
                    if (currentDest == null)
                    {
                        if ((i = checks.FindIndex(o => string.Equals(currPos, o, StringComparison.OrdinalIgnoreCase))) > -1)
                        {
                            status = "Checkpoint " + checks[i] + " reached. "; //print i'm on a checkpoint
                            checks.RemoveAt(i);
                        }
                    }
                    else
                    {
                        if (currPos.Equals(target))
                        {
                            status = "Target " + target + " reached. "; //PRINT Arrived on target currPos
                            setLock(false);
                            target = null;
                            return "NULL";
                        }
                        else status = "Checkpoint " + currPos + " reached. "; //Print arrived on checkpoint currPos
                    }
                    //QUI
                }
                else if ((i = checks.FindIndex(o => string.Equals(currPos, o, StringComparison.OrdinalIgnoreCase))) > -1)
                {
                    status = "Checkpoint " + currPos + " reached. Heading to " + currentDest; //print i'm on a checkpoint
                    checks.RemoveAt(i); //if casually i'm on a checkpoint remove it
                }
                else status = "Heading to " + currentDest;

                // Next target selection
                selectLocalTarget();

                // Find route step by step (to check)
                findRoute(currPos, dir);
            }
            
            return getOperation();
        }

        private void selectLocalTarget()
        {
            // Actual strategy: Most distant from target first (target will be the last)
            // Ideal strategy: TSP, Travelling Salesman Problem

            string winner = target;
            int distance = 0;
            int Trow = (int)target[0];
            int Tcol = (int)target[1];
            foreach (var str in checks)
            {
                int row = (int)str[0];
                int col = (int)str[1];
                int newDist = Math.Abs(Trow - row) + Math.Abs(Tcol - col);
                if (newDist > distance)
                {
                    distance = newDist;
                    winner = str;
                }
            }
            // Set current destination
            currentDest = winner;
            if (checks.Count > 0) checks.Remove(winner);
            status += "Heading to " + currentDest;
        }

        // Enqueue the operations needed to reach currentDest in the database
        private void findRoute(string currPos, Dir dir)
        {
            // calculate angle as displacement
            float angle = picture.getAngle();
            int Trow = (int)currentDest[0];
            int Tcol = (int)currentDest[1];
            int CProw = (int)currPos[0];
            int CPcol = (int)currPos[1];
            if(angle == -1)
            {
                switch (dir)
                {
                    case Dir.NORTH:
                        angle = 0;
                        break;

                    case Dir.EAST:
                        angle = 90;
                        break;
                    case Dir.SOUTH:
                        angle = 180;
                        break;
                    default:
                        angle = 270;
                        break;
                }
            }
            string.Format("{0:000}", angle);
            if (Trow - CProw > 0)
            {
                angle = (540 - angle) % 360;
                if (angle > 180)
                    angle = angle - 180;
                //ALERT! BACKWARD2 used because no BACKWARD was found
                status += "POS:"+currPos+" DIR:" + dir.ToString();
                switch (dir)
                {
                    case Dir.NORTH:
                        addOperation("B" + angle.ToString());
                        break;
                    case Dir.SOUTH:
                        addOperation("F" + angle.ToString());
                        break;
                    case Dir.EAST:
                        addOperation("R" + angle.ToString());
                        break;
                    default:
                        addOperation("L" + angle.ToString());
                        break;
                }
                currDir = Dir.SOUTH;
                currPos = (char)(CProw + 1) + ""+ (char)CPcol; 
            }
            else if (Trow - CProw < 0)
            {
                angle = (360 -angle) % 360;
                if (angle > 180)
                    angle = angle - 180;
                switch (dir)
                {
                    case Dir.NORTH:
                        addOperation("F" + angle.ToString());
                        break;
                    case Dir.SOUTH:
                        addOperation("B" + angle.ToString());
                        break;
                    case Dir.EAST:
                        addOperation("L" + angle.ToString());
                        break;
                    default:
                        addOperation("R" + angle.ToString());
                        break;
                }
                currPos = (char)(CProw - 1) + "" + (char)CPcol;
                currDir = Dir.NORTH;
            }
            else if (Tcol - CPcol > 0)
            {
                angle = (450 - angle) % 360;
                if (angle > 180)
                    angle = angle - 180;
                //ALERT! BACKWARD2 used because no BACKWARD was found
                switch (dir)
                {
                    case Dir.NORTH:
                        addOperation("R" + angle.ToString());
                        break;
                    case Dir.SOUTH:
                        addOperation("L" + angle.ToString());
                        break;
                    case Dir.EAST:
                        addOperation("F" + angle.ToString());
                        break;
                    default:
                        addOperation("B" + angle.ToString());
                        break;
                }
                currDir = Dir.EAST;
                currPos = (char)CProw + "" + (char)(CPcol + 1);
            }
            else if(Tcol - CPcol < 0)
            {
                angle = (630 - angle) % 360;
                if (angle > 180)
                    angle = angle - 180;
                switch (dir)
                {
                    case Dir.NORTH:
                        addOperation("L" + angle.ToString());
                        break;
                    case Dir.SOUTH:
                        addOperation("R" + angle.ToString());
                        break;
                    case Dir.EAST:
                        addOperation("B" + angle.ToString());
                        break;
                    default:
                        addOperation("F" + angle.ToString());
                        break;
                }
                currDir = Dir.WEST;
                currPos = (char)CProw + "" + (char)(CPcol - 1);
            }
        }
 
    }
}  