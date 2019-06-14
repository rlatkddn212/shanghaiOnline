package prodconsumselectserver;

import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;

public class Item {
	SelectionKey key;
	ByteBuffer data;
	public Item(SelectionKey key ,ByteBuffer data) {
		// TODO Auto-generated constructor stub
		this.key = key;
		this.data = data;
	}
	
	public synchronized SelectionKey getKey(){
		return key;
	}
	
	public synchronized ByteBuffer getData(){
		return data;
	}
}
