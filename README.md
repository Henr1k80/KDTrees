# K-D Trees

This repo was made after I read an online post about K-D Trees on linkedin.

Program.cs performs a test.
A random map is created with 10mio points.
10k random points are generated in a list.
Each of these points are run against the TreeStrategy which will then find the closest point(s) on the map for each input point.
This entire run takes below 90ms.

Pros of K-D Trees: Once indexing is done, it is lightning fast to check the closest point to an input point.
Cons: The indexing has to be redone of the points in the map move around. In gaming this is often the case, and quadtrees/octettrees are more optimal here.
