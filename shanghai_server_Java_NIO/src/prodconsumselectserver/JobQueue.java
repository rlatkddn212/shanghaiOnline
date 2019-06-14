package prodconsumselectserver;

import java.util.LinkedList;
import java.util.NoSuchElementException;

public class JobQueue implements Queue {

	private static final String NAME = "JOB QUEUE";
	private static final Object monitor = new Object();

	private LinkedList jobs = new LinkedList();

	private static JobQueue instance = new JobQueue();

	private JobQueue() {
	}

	public static JobQueue getInstance() {
		if (instance == null) {
			synchronized (JobQueue.class) {
				instance = new JobQueue();
			}
		}
		return instance;
	}

	@Override
	public String getName() {
		// TODO Auto-generated method stub
		return NAME;
	}

	@Override
	public void clear() {
		// TODO Auto-generated method stub
		synchronized (monitor) {
			jobs.clear();
		}
	}

	public void put(Object o) {
		synchronized (monitor) {
			jobs.addLast(o);
			monitor.notify();
		}
	}

	@Override
	public Object pop() throws InterruptedException, NoSuchElementException {
		// TODO Auto-generated method stub
		Object o = null;
		synchronized (monitor) {
			if (jobs.isEmpty()) {
				monitor.wait();
			}
			o = jobs.removeFirst();
		}
		if (o == null)
			throw new NoSuchElementException();
		return o;
	}

	@Override
	public int size() {
		// TODO Auto-generated method stub
		return jobs.size();
	}

}
