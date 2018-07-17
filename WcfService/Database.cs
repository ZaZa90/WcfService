using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
//commit?
// Can be improved replacing Operation with string coded as Lddd where L is a letter and ddd are 3 digits describing the angle
// For ex. R127 will be rotation RIGHT by 127 degree 
public enum Operation{STOP,FORWARD,RIGHT,LEFT,PICTURE,NULL,ERROR,FORWARD2,BACKWARD2,RIGHT2,LEFT2};

public enum Dir { NORTH, SOUTH, EAST, WEST };

namespace WcfService
{
    public class Database
    {
        static string ip = "0.0.0.0";
        static Picture picture;
        static int pictureId = 0;
        static Queue<Operation> operations = new Queue<Operation>();

        static string target = null;
        static List<string> checks;
        static Operation currentOp;
        static string currentDest;
        static string status;
        static bool dbLock = false;

        public Queue<Operation> getOperations() { return operations; }

        public void addOperation(Operation op) { operations.Enqueue(op); }

        public Operation getOperation() {
            if (operations.Count > 0)
            {
                currentOp = readOperation();
                return operations.Dequeue();
            }
            else
                return Operation.NULL;
        }

        public Operation readOperation()
        {
            if (operations.Count > 0)
                return operations.ElementAt(0);
            else
                return Operation.NULL;
        }

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
        public Operation getNextLocalTarget()
        {
            int i;
            string currPos = picture.getQRCode();
            float angle = picture.getAngle();
            Dir dir = (angle > 45 && angle <= 135) ? Dir.WEST : (angle > 135 && angle <= 225) ? Dir.SOUTH : (angle > 225 && angle <= 315) ? Dir.EAST : Dir.NORTH; 
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
                            return Operation.NULL;
                        }
                        else status = "Checkpoint " + currPos + " reached. "; //Print arrived on checkpoint currPos
                    }
                    // Next target selection
                    selectLocalTarget();

                    // Find route
                    findRoute(currPos, dir);
                }
                else if ((i = checks.FindIndex(o => string.Equals(currPos, o, StringComparison.OrdinalIgnoreCase))) > -1)
                {
                    status = "Checkpoint " + currPos + " reached. Heading to " + currentDest; //print i'm on a checkpoint
                    checks.RemoveAt(i); //if casually i'm on a checkpoint remove it
                }
                else status = "Heading to " + currentDest;
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
            Dir newDir;
            int Trow = (int)currentDest[0];
            int Tcol = (int)currentDest[1];
            int CProw = (int)currPos[0];
            int CPcol = (int)currPos[1];
            if (Trow - CProw > 0)
            {
                //ALERT! BACKWARD2 used because no BACKWARD was found
                switch (dir)
                {
                    case Dir.NORTH:
                        addOperation(Operation.BACKWARD2);
                        break;
                    case Dir.SOUTH:
                        addOperation(Operation.FORWARD);
                        break;
                    case Dir.EAST:
                        addOperation(Operation.RIGHT);
                        break;
                    default:
                        addOperation(Operation.LEFT);
                        break;
                }
                newDir = Dir.SOUTH;
            }
            else if (Trow - CProw < 0)
            {
                switch (dir)
                {
                    case Dir.NORTH:
                        addOperation(Operation.FORWARD);
                        break;
                    case Dir.SOUTH:
                        addOperation(Operation.BACKWARD2);
                        break;
                    case Dir.EAST:
                        addOperation(Operation.LEFT);
                        break;
                    default:
                        addOperation(Operation.RIGHT);
                        break;
                }
                newDir = Dir.NORTH;
            }
            else newDir = dir;
            for (int i = 1; i < Math.Abs(Trow - CProw); i++)
            {
                addOperation(Operation.FORWARD);
            }

            if (Tcol - CPcol > 0)
            {
                //ALERT! BACKWARD2 used because no BACKWARD was found
                switch (newDir)
                {
                    case Dir.NORTH:
                        addOperation(Operation.RIGHT);
                        break;
                    case Dir.SOUTH:
                        addOperation(Operation.LEFT);
                        break;
                    case Dir.EAST:
                        addOperation(Operation.FORWARD);
                        break;
                    default:
                        addOperation(Operation.BACKWARD2);
                        break;
                }
            }
            else if(Tcol - CPcol < 0)
            {
                switch (newDir)
                {
                    case Dir.NORTH:
                        addOperation(Operation.LEFT);
                        break;
                    case Dir.SOUTH:
                        addOperation(Operation.RIGHT);
                        break;
                    case Dir.EAST:
                        addOperation(Operation.BACKWARD2);
                        break;
                    default:
                        addOperation(Operation.FORWARD);
                        break;
                }
            }
            for (int i = 1; i < Math.Abs(Tcol - CPcol); i++)
            {
                addOperation(Operation.FORWARD);
            }

        }

        public string getStatus()
        {
            return status;
        }

        public Operation getCurrentOperation()
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
    }
}  