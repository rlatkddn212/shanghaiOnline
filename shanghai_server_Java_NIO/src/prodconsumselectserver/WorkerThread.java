package prodconsumselectserver;

import java.io.IOException;
import java.net.ServerSocket;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.charset.Charset;
import java.nio.charset.CharsetDecoder;
import java.util.Iterator;
import java.util.NoSuchElementException;
import java.util.logging.FileHandler;

public class WorkerThread extends Thread {
	Queue queue;
	FileHandler fileHandler;
	ServerSocketChannel serverSocketChannel;
	ServerSocket serverSocket;
	private CharsetDecoder decoder = null;
	private Charset charset = null;

	public WorkerThread(Queue queue) throws IOException {
		// TODO Auto-generated constructor stub
		this.queue = queue;
		charset = Charset.forName("UTF-8");
		decoder = charset.newDecoder();
	}

	public void run() {
		try {
			while (!Thread.currentThread().isInterrupted()) {
				System.out.println(AdvancedServer.selector);
				// AdvancedServer.selector.select();
				Object o = queue.pop();

				Item i = (Item) o;
				SelectionKey key = i.getKey();
				ByteBuffer data = i.getData();

				System.out.println(key);

				SocketChannel sc = (SocketChannel) key.channel();
				try {
					System.out.println(decoder.decode(data).toString());

					broadcast(sc, data);
				} catch (IOException e) {
					closeChannel(sc);

				}

			}
		} catch (NoSuchElementException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private void broadcast(SocketChannel sc, ByteBuffer data)
			throws IOException {

		try {
			for (int i = 0; i < 2; i++) {
				sc.read(data);
				System.out.println("버퍼로 보내는 값 " + data);
			}

			data.flip();

			Iterator iter = GameRoomUser.getInstance().iterator();
			while (iter.hasNext()) {
				SocketChannel member = (SocketChannel) iter.next();
				if (member != null && member.isConnected()) {
					while (data.hasRemaining()) {
						member.write(data);
					}
					data.rewind();
				}
			}
		} finally {
		}
	}

	private void closeChannel(SocketChannel sc) {
		try {
			sc.close();
			GameRoomUser.getInstance().remove(sc);
		} catch (IOException e) {
		}
	}
}
