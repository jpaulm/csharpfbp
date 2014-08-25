using System;
using System.Threading;

namespace Spring.Threading.Helpers
{
    /// <summary> 
    /// A synchronization aid that allows one or more threads to wait until
    /// a set of operations being performed in other threads completes.
    /// </summary>
    /// <remarks>
    /// A <see cref="Spring.Threading.Helpers.CountDownLatch"/> is initialized with a given
    /// <b>count</b>.  The <see cref="Spring.Threading.Helpers.CountDownLatch.Await()"/> and <see cref="Spring.Threading.Helpers.CountDownLatch.Await(TimeSpan)"/>
    /// methods block until the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/> 
    /// reaches zero due to invocations of the
    /// <see cref="Spring.Threading.Helpers.CountDownLatch.CountDown()"/> method, after which all waiting threads are
    /// released and any subsequent invocations of <see cref="Spring.Threading.Helpers.CountDownLatch.Await()"/> and <see cref="Spring.Threading.Helpers.CountDownLatch.Await(TimeSpan)"/> return
    /// immediately. This is a one-shot phenomenon -- the count cannot be
    /// reset.  If you need a version that resets the count, consider using
    /// a <see cref="Spring.Threading.CyclicBarrier"/>.
    /// 
    /// <p/>
    /// A <see cref="Spring.Threading.Helpers.CountDownLatch"/> is a versatile synchronization tool
    /// and can be used for a number of purposes.  A
    /// <see cref="Spring.Threading.Helpers.CountDownLatch"/> initialized with a count of one serves as a
    /// simple on/off latch, or gate: all threads invoking  <see cref="Spring.Threading.Helpers.CountDownLatch.Await()"/> and <see cref="Spring.Threading.Helpers.CountDownLatch.Await(TimeSpan)"/>
    /// wait at the gate until it is opened by a thread invoking <see cref="Spring.Threading.Helpers.CountDownLatch.CountDown()"/>.
    /// A <see cref="Spring.Threading.Helpers.CountDownLatch"/> initialized to <i>N</i>
    /// can be used to make one thread wait until <i>N</i> threads have
    /// completed some action, or some action has been completed <i>N</i> times.
    /// 
    /// <p/>
    /// A useful property of a <see cref="Spring.Threading.Helpers.CountDownLatch"/> is that it
    /// doesn't require that threads calling <see cref="Spring.Threading.Helpers.CountDownLatch.CountDown()"/> wait for
    /// the count to reach zero before proceeding, it simply prevents any
    /// thread from proceeding past an <see cref="Spring.Threading.Helpers.CountDownLatch.Await()"/> and <see cref="Spring.Threading.Helpers.CountDownLatch.Await(TimeSpan)"/> until all
    /// threads could pass.
    /// 
    /// <p/>
    /// <b>Sample usage:</b> 
    /// <br/>
    /// Here is a pair of classes in which a group
    /// of worker threads use two countdown latches:
    /// <ul>
    /// <li>The first is a start signal that prevents any worker from proceeding
    /// until the driver is ready for them to proceed.</li>
    /// <li>The second is a completion signal that allows the driver to wait
    /// until all workers have completed.</li>
    /// </ul>
    /// 
    /// <code>
    /// internal class Driver { // ...
    ///		void Main() {
    ///			CountDownLatch startSignal = new CountDownLatch(1);
    ///			CountDownLatch doneSignal = new CountDownLatch(N);
    /// 
    ///			for (int i = 0; i &lt; N; ++i)
    ///				new Thread(new ThreadStart(new Worker(startSignal, doneSignal).Run).Start();
    /// 
    /// 		doSomethingElse();            // don't let run yet
    ///			startSignal.CountDown();      // let all threads proceed
    /// 		doSomethingElse();
    /// 		doneSignal.Await();           // wait for all to finish
    /// 	}
    /// }
    /// 
    /// internal class Worker : IRunnable {
    ///		private CountDownLatch startSignal;
    /// 	private CountDownLatch doneSignal;
    /// 	Worker(CountDownLatch startSignal, CountDownLatch doneSignal) {
    /// 		this.startSignal = startSignal;
    /// 		this.doneSignal = doneSignal;
    /// 	}
    /// 	internal void Run() {
    /// 		try {
    /// 			startSignal.Await();
    /// 			doWork();
    /// 			doneSignal.CountDown();
    /// 		} catch (ThreadInterruptedException ex) {} // return;
    /// 	}
    /// 
    /// 	void doWork() { ... }
    /// }
    /// 
    /// </code>
    /// 
    /// <p/>
    /// Another typical usage would be to divide a problem into N parts,
    /// describe each part with a <see cref="Spring.Threading.IRunnable"/> that executes that portion and
    /// counts down on the latch, and queue all the <see cref="Spring.Threading.IRunnable"/>s to an
    /// <see cref="Spring.Threading.IExecutor"/>.  When all sub-parts are complete, the coordinating thread
    /// will be able to pass through await. (When threads must repeatedly
    /// count down in this way, instead use a <see cref="Spring.Threading.CyclicBarrier"/>.)
    /// 
    /// <code>
    /// internal class Driver2 { // ...
    ///		void Main() {
    ///			CountDownLatch doneSignal = new CountDownLatch(N);
    ///			Executor e = ...
    /// 
    /// 		for (int i = 0; i &lt; N; ++i) // create and start threads
    /// 				e.execute(new WorkerRunnable(doneSignal, i));
    /// 
    ///			doneSignal.await();           // wait for all to finish
    ///		}
    /// }
    /// 
    /// internal class WorkerRunnable : IRunnable {
    ///		private CountDownLatch doneSignal;
    /// 	private int i;
    /// 	WorkerRunnable(CountDownLatch doneSignal, int i) {
    /// 		this.doneSignal = doneSignal;
    /// 		this.i = i;
    /// 	}
    /// 	internal void Run() {
    /// 		try {
    /// 			doWork(i);
    /// 			doneSignal.CountDown();
    /// 		} catch (ThreadInterruptedException ex) {} // return;
    /// 	}
    ///	 
    ///		void doWork() { ... }
    /// }
    /// 
    /// </code>
    /// </remarks>
    /// <author>Doug Lea</author>
    /// <author>Griffin Caprio (.NET)</author>
    public class CountDownLatch
    {
        private int _count;

        /// <summary> 
        /// Returns the current count.
        /// </summary>
        /// <remarks>
        /// This method is typically used for debugging and testing purposes.
        /// </remarks>
        /// <returns>the current count.</returns>
        internal long Count
        {
            get { return _count; }

        }
        /// <summary> 
        /// Constructs a <see cref="Spring.Threading.Helpers.CountDownLatch"/> initialized with the given
        /// <paramref name="count"/>.
        /// </summary>
        /// <param name="count">the number of times <see cref="Spring.Threading.Helpers.CountDownLatch.CountDown"/> must be invoked
        /// before threads can pass through <see cref="Spring.Threading.Helpers.CountDownLatch.Await()"/>.
        /// </param>
        /// <exception cref="System.ArgumentException">if <paramref name="count"/> is less than 0.</exception>
        internal CountDownLatch(int count)
        {
            if (count < 0)
                throw new ArgumentException("Count must be greater than 0.");
            _count = count;
        }

        /// <summary> 
        /// Causes the current thread to wait until the latch has counted down to
        /// zero, unless <see cref="System.Threading.Thread.Interrupt()"/> is called on the thread.
        /// </summary>	
        /// <remarks>
        /// If the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/>  is zero then this method
        /// returns immediately.
        /// <p/>If the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/> is greater than zero then
        /// the current thread becomes disabled for thread scheduling
        /// purposes and lies dormant until the count reaches zero due to invocations of the
        /// <see cref="Spring.Threading.Helpers.CountDownLatch.CountDown()"/> method or 
        /// some other thread calls <see cref="System.Threading.Thread.Interrupt()"/> on the current
        /// thread.
        /// <p/>
        /// A <see cref="System.Threading.ThreadInterruptedException"/> is thrown if the thread is interrupted.
        /// </remarks>
        /// <exception cref="System.Threading.ThreadInterruptedException">if the current thread is interrupted.</exception>
        internal void Await()
        {
            lock (this)
            {
                while (_count > 0)
                     Monitor.Wait(this); 
            }
        }

        /// <summary> 
        /// Causes the current thread to wait until the latch has counted down to
        /// zero, unless <see cref="System.Threading.Thread.Interrupt()"/> is called on the thread or
        /// the specified <paramref name="duration"/> elapses.
        /// </summary>	
        /// <remarks> 
        /// <p/>
        /// If the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/>  is zero then this method
        /// returns immediately.
        /// <p/>If the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/> is greater than zero then
        /// the current thread becomes disabled for thread scheduling
        /// purposes and lies dormant until the count reaches zero due to invocations of the
        /// <see cref="Spring.Threading.Helpers.CountDownLatch.CountDown()"/> method or 
        /// some other thread calls <see cref="System.Threading.Thread.Interrupt()"/> on the current
        /// thread.
        /// <p/>
        /// A <see cref="System.Threading.ThreadInterruptedException"/> is thrown if the thread is interrupted.
        /// <p/>
        /// If the specified <paramref name="duration"/> elapses then the value <see lang="false"/>
        /// is returned. If the time is less than or equal to zero, the method will not wait at all.
        /// </remarks>
        /// <param name="duration">the maximum time to wait
        /// </param>
        /// <returns> <see lang="true"/> if the count reached zero  and <see lang="false"/>
        /// if the waiting time elapsed before the count reached zero.
        /// </returns>
        /// <exception cref="System.Threading.ThreadInterruptedException">if the current thread is interrupted.</exception>
        internal bool Await(TimeSpan duration)
        {
            TimeSpan durationToWait = duration;
            lock (this)
            {
                if (_count <= 0)
                    return true;
                else if (durationToWait.Ticks <= 0)
                    return false;
                else
                {
                    DateTime deadline = DateTime.Now.Add(durationToWait);
                    for (; ; )
                    {
                        Monitor.Wait(this, durationToWait);
                        if (_count <= 0)
                            return true;
                        else
                        {
                            durationToWait = deadline.Subtract(DateTime.Now);
                            if (durationToWait.Ticks <= 0)
                                return false;
                        }
                    }
                }
            }
        }

        /// <summary> 
        /// Decrements the count of the latch, releasing all waiting threads if
        /// the count reaches zero.
        /// </summary>
        /// <remarks>
        /// If the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/> is greater than zero then
        /// it is decremented. If the new count is zero then all waiting threads
        /// are re-enabled for thread scheduling purposes. If the current <see cref="Spring.Threading.Helpers.CountDownLatch.Count"/> equals zero then nothing
        /// happens.
        /// </remarks>
        internal void CountDown()
        {
            lock (this)
            {
                if (_count == 0)
                    return;
                if (--_count == 0)
                    Monitor.PulseAll(this);
            }
        }

        /// <summary> 
        /// Returns a string identifying this latch, as well as its state.
        /// </summary>
        /// <remarks>
        /// The state, in brackets, includes the String
        /// &quot;Count =&quot; followed by the current count.
        /// </remarks>
        /// <returns> a string identifying this latch, as well as its state</returns>
        public override string ToString()
        {
            return base.ToString() + "[Count = " + Count + "]";
        }
    }
}