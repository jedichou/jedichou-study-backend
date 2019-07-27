// Listing 3-23. Manually Synchronizing a First-Generation Collection Class

using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;

namespace Listing0323
{
	class Program
	{
		static void Main(string[] args)
		{
			// create a collection
			var sharedQueue = new Queue();

			// populate the collection with items to process
			for (int i = 0; i < 1000; i++)
				sharedQueue.Enqueue(i);

			// define a counter for the number of processed items
			var itemCount = 0;

			// create tasks to process the list
			var tasks = new Task[10];
			for (int i = 0; i < tasks.Length; i++) {
				// create the new task
				tasks[i] = new Task( ()=> {
					while (sharedQueue.Count > 0) {
						// get the lock using the collections sync root
						lock (sharedQueue.SyncRoot) {
							if (sharedQueue.Count > 0) {
								// take an item from the queue
								var queueElement = (int)sharedQueue.Dequeue();
								// increment the count of items processed
								Interlocked.Increment(ref itemCount);
							}
						}
					}
				});
				// start the new task
				tasks[i].Start();
			}
			
			// wait for the tasks to complete
			Task.WaitAll(tasks);

			// report on the number of items processed
			Console.WriteLine("Items processed: {0}", itemCount);
		}
	}
}
