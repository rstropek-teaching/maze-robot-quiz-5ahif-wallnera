using Maze.Library;
using System.Collections.Generic;
using System.Drawing;

namespace Maze.Solver
{
    /// <summary>
    /// Moves a robot from its current position towards the exit of the maze
    /// </summary>
    public class RobotController
    {
        private IRobot robot;
        // why hashset?
        // https://stackoverflow.com/questions/4558754/define-what-is-a-hashset 
        // -> says it is quick and good for searching 
        protected HashSet<Point> visited = new HashSet<Point>();
        protected bool end = false; // he isn't at the end in the beginning...
        /// <summary>
        /// Initializes a new instance of the <see cref="RobotController"/> class
        /// </summary>
        /// <param name="robot">Robot that is controlled</param>
        public RobotController(IRobot robot)
        {
            // Store robot for later use
            this.robot = robot;
        }

        /// <summary>
        /// Moves the robot to the exit
        /// </summary>
        /// <remarks>
        /// This function uses methods of the robot that was passed into this class'
        /// constructor. It has to move the robot until the robot's event
        /// <see cref="IRobot.ReachedExit"/> is fired. If the algorithm finds out that
        /// the exit is not reachable, it has to call <see cref="IRobot.HaltAndCatchFire"/>
        /// and exit.
        /// </remarks>
        public void MoveRobotToExit()
        {
            // Here you have to add your code
            Point initpoint = new Point(0, 0);  // Why Point? -> x,y
            visited = new HashSet<Point>();

            robot.ReachedExit += (_, __) => end = true;
            // solve maze
            recursiveVisitField(initpoint);
            // he returned without solution
            // our robot trapped :(
            if (end == false)
                robot.HaltAndCatchFire(); 

            // not very glorious code
            //while (!reachedEnd)
            //{
            //    if (robot.CanIMove(Direction.Right))
            //    {
            //        // it can move right
            //        robot.Move(Direction.Right); // he moved
            //    }

            //    if (robot.CanIMove(Direction.Left))
            //    {
            //        // it can move left
            //        robot.Move(Direction.Left); // he moved
            //    }

            //    if (robot.CanIMove(Direction.Down))
            //    {
            //        // it can move down
            //        robot.Move(Direction.Down); // he moved
            //    }

            //    if (robot.CanIMove(Direction.Up))
            //    {
            //        // it can move up
            //        robot.Move(Direction.Up); // he moved
            //    }

            //    if (!robot.CanIMove(Direction.Up) && !robot.CanIMove(Direction.Down) && !robot.CanIMove(Direction.Left) && !robot.CanIMove(Direction.Right))
            //    {
            //        // it is trapped :(
            //        robot.HaltAndCatchFire();
            //    }
              //}
            // Trivial sample algorithm that can just move right
            //var reachedEnd = false;
            //robot.ReachedExit += (_, __) => reachedEnd = true;

            //while (!reachedEnd)
            //{
            //    robot.Move(Direction.Right);
            //}
        }
        /// <summary>
        /// checks for exit
        /// </summary>
        /// <param name="testpoint"></param>
        public void recursiveVisitField(Point testpoint)
        {
            // check if he has never been here and is not at the end
            if (visited.Contains(testpoint)==false && end==false)
            {
                // visited testpoint
                visited.Add(testpoint); 
                // he can move left
                if (robot.TryMove(Direction.Left) == true)
                {
                    // check point left
                    Point newtestpoint = new Point(testpoint.X - 1, testpoint.Y);
                    // try another point 
                    recursiveVisitField(newtestpoint);
                    // move back to origin
                    if (end == false) { robot.Move(Direction.Right); }
                }

                if(robot.TryMove(Direction.Right) == true)
                {
                    // check point right
                    Point newtestpoint = new Point(testpoint.X + 1, testpoint.Y);
                    recursiveVisitField(newtestpoint);
                    if (end == false) { robot.Move(Direction.Left); }
                }

                if (robot.TryMove(Direction.Down) == true)
                {
                    // check point below
                    Point newtestpoint = new Point(testpoint.X, testpoint.Y + 1);
                    recursiveVisitField(newtestpoint);
                    if (end == false) { robot.Move(Direction.Up); }
                }

                if (robot.TryMove(Direction.Up) == true)
                {
                    // check point above
                    Point newtestpoint = new Point(testpoint.X, testpoint.Y - 1);
                    recursiveVisitField(newtestpoint);
                    if (end == false) { robot.Move(Direction.Down);  }
                }
            }
        }
    }
}
