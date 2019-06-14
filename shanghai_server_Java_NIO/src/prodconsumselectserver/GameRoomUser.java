package prodconsumselectserver;

import java.nio.channels.SocketChannel;
import java.util.Vector;

public class GameRoomUser extends Vector<SocketChannel>{
	private static GameRoomUser cr = new GameRoomUser();
	private GameRoomUser(){
	}
	
	public static GameRoomUser getInstance(){
		
		return cr;
	}
}
