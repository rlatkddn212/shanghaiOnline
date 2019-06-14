package prodconsumselectserver;
import java.util.NoSuchElementException;


public interface Queue {
	public String getName();
	public void clear();
	public void put(Object o);
	
	public Object pop() throws InterruptedException, NoSuchElementException;
	public int size();
}
