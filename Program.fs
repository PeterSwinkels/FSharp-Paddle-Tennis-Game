//F Ball - by: Peter Swinkels, ***2018***

//The namespaces used by this program.
open System
open System.Drawing

//This module contains this program's core functions.
module private CoreModule =
   //This record defines the ball.
   type private ball = 
      {
         direction: Point  //The ball's direction.
         last: int         //The last movement's interval.
         xy: Point         //The ball's position.
      }

   //This function manages the ball's direction.
   let private getDirection (xy:Point) (direction:Point) = 
      new Point(
         match xy.X with
         |  1 -> +1
         | 78 -> -1
         | _  -> direction.X
         ,
         match xy.Y with
         |  1 -> +1
         | 23 -> -1
         | _  -> direction.Y
      )
  
   //This function manages the ball's position.
   let private getPosition (xy:Point) (direction:Point) = 
      new Point(xy.X + direction.X, xy.Y + direction.Y)
   
   //This function gives the command to move the ball at specific time intervals.
   let rec private interval ball x score = 
      (0, 0) |> Console.SetCursorPosition
      printf "%A" score
  
      if Console.KeyAvailable then        
         let key = true |> Console.ReadKey
 
         if key.Key = ConsoleKey.Escape then
            0
         else if key.Key = ConsoleKey.LeftArrow then
            (x, 23) |> Console.SetCursorPosition
            printf "    "
            interval ball (if x > 0 then x - 1 else x) score
         else if key.Key = ConsoleKey.RightArrow then
            (x, 23) |> Console.SetCursorPosition
            printf "    "            
            interval ball (if x < 76 then x + 1 else x) score
         else
            interval ball x score
      else
         if Environment.TickCount >= ball.last + 125 || Environment.TickCount <= 250 then
            let newBall = {direction = getDirection ball.xy ball.direction; last = Environment.TickCount; xy = getPosition ball.xy ball.direction}
            let newScore = if ball.xy.Y = 23 && ball.xy.X >= x && ball.xy.X <= x + 4 then score + 1 else score
            (ball.xy.X, ball.xy.Y) |> Console.SetCursorPosition
            printf " "
            (x, 23) |> Console.SetCursorPosition
            printf "===="
            (newBall.xy.X, newBall.xy.Y) |> Console.SetCursorPosition
            printf "*"
            interval newBall x newScore
         else
            interval ball x score
   
   //This function is executed when this program is started.
   [<EntryPoint>]
   let private main argv = 
      Console.CursorVisible <- false
      (80, 25) |> Console.SetBufferSize     
      interval {direction = new Point(1, 1); xy = new Point(0, 0); last = Environment.TickCount} 38 0
