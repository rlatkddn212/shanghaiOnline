package prodconsumselectserver;

import java.io.IOException;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.nio.ByteBuffer;
import java.nio.channels.SelectionKey;
import java.nio.channels.Selector;
import java.nio.channels.ServerSocketChannel;
import java.nio.channels.SocketChannel;
import java.nio.charset.Charset;
import java.nio.charset.CharsetDecoder;
import java.util.Iterator;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.logging.FileHandler;

public class AdvancedServer {

	FileHandler fileHandler;
	public static Selector selector;
	ServerSocketChannel serverSocketChannel;
	ServerSocket serverSocket;
	private CharsetDecoder decoder = null;
	private Charset charset = null;
	private static final ExecutorService threadPool = Executors.newFixedThreadPool(3);
	// 키값을
	private Queue queue = null;

	// 초기화 쓰레드 생성
	public AdvancedServer() {
		// TODO Auto-generated constructor stub
		queue = JobQueue.getInstance();
		charset = Charset.forName("UTF-8");
		decoder = charset.newDecoder();
		initServer();
		startServer();

	}

	private void startServer() {
		// TODO Auto-generated method stub
		while (true) {

			try {
				selector.select();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

			Iterator it = selector.selectedKeys().iterator();
			
			while (it.hasNext()) {
				SelectionKey key = (SelectionKey) it.next();

				try {

					if (key.isAcceptable()) {
						ServerSocketChannel server = (ServerSocketChannel) key
								.channel();
						SocketChannel sc;

						sc = server.accept();
						sc.configureBlocking(false);
						sc.register(selector, SelectionKey.OP_READ);
						GameRoomUser.getInstance().add(sc);
					}
					// accept(key); // System.out.println(key); } else if
					if (key.isReadable()) {
						SocketChannel sc;
						sc = (SocketChannel) key.channel();
						ByteBuffer buffer = ByteBuffer.allocateDirect(1024);
						int numRead = 0;
						// sc.read(key);
						try{
							numRead = sc.read(buffer);
						}catch(IOException e){
							sc.close();
						}
						
						if (numRead == -1) {
							key.channel().close();
							key.cancel();
							return;
						}
						buffer.flip();
						//System.out.println(decoder.decode(buffer).toString());
						
						queue.put(new Item(key,buffer));

					}
					it.remove();
					// 처리한 이벤트 삭제
				} catch (IOException e) {
					// TODO Auto-generated catch block
					
					e.printStackTrace();
				}
			}

		}

	}

	private void initServer() {
		// TODO Auto-generated method stub
		try {

			selector = Selector.open();
			serverSocketChannel = ServerSocketChannel.open();
			// 넌 블럭킹 모드
			serverSocketChannel.configureBlocking(false);

			serverSocket = serverSocketChannel.socket();

			InetSocketAddress isa = new InetSocketAddress("127.0.0.1", 9090);

			serverSocket.bind(isa);

			serverSocketChannel.register(selector, SelectionKey.OP_ACCEPT);

			threadPool.execute(new WorkerThread(queue));

			System.out.println("서버 시작");
		} catch (IOException e) {

		}
	}

}
