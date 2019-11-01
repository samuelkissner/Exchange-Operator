# Term Project Proposal

## OBJECTIVE

By implementing my own version of the exchange operator model of parallelization, I hope to gain a deeper understanding of the topic, and know it well enough to teach my fellow classmates via the reading review presentation at the end of the semester.

## METHODOLOGY

### Possible Questions

* List the four ways in which the exchange operator can partition data. Explain when each of these four ways would be used.

* What are the benefits of the exchange operator model?

* Create a concrete example (including tuples) that implements parallel external sort-merge as a series of exchange operations followed by local query operations. Show intermediate results at each step.

### Project Description Steps

1. Simulate a parallel processing environment via a Node class (written in C#). Every instance of this class will represent a different Node in a parallel processing environment

1. Create an exchange operation function in C# that can partition input data via hash partitioning, range partitioning, broadcasting, and by sending all data to a single node.

1. Implement the following parallel processing functions in C# using the exchange operation function (developed earlier) to partition data: 
	* Range partitioning sort 
	* Parallel external sort-merge 
	* Partitioned join
	* Asymmetric fragment-and-replicate join
	* Symmetric fragment-and-replicate join
	* Aggregation

1. Create a large set of example tuples

1. Create a driver program in C# that will perform the various parallel processing functions on the set of example tuples.

## EXPECTED OUTCOME

I expect I’ll be able to develop a program that implements the various parallel processing operations using the exchange operator model. The program will output (to the console) intermediate steps that show the state of the tuples at each of the nodes.

