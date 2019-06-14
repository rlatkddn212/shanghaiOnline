package prodconsumselectserver;
import java.sql.*;

/***
 * 
 * @author rlatkddn212
 * 
 */
// ?±Í??¥ÏúºÎ°?Íµ¨ÌòÑ??DB
public class DBcon {
	private static DBcon instance = new DBcon();
	Connection conn;

	private DBcon() {
	}

	public static DBcon getInstance() {
		return instance;
	}

	public void startConn() {
		if (conn != null) {
			return;
		}

		try {
			String jdbc = "jdbc:mysql://localhost:3306/shang";
			String id = "shanghai";
			String pass = "infonet";

			Class.forName("com.mysql.jdbc.Driver");

			conn = DriverManager.getConnection(jdbc, id, pass);
			System.out.println("?îÎπÑ?∞Í≤∞ ?±Í≥µ");
		} catch (ClassNotFoundException e) {
		} catch (SQLException e) {
			System.out.println("?îÎπÑ?∞Í≤∞ ?§Ìå®");
		}
	}

	public void DBDiscon() {
		try {
			conn.close();
		} catch (SQLException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	public void InsertUser(DBproperty user) {

		PreparedStatement pstmt = null;
		try {

			pstmt = conn
					.prepareStatement("insert into user values (?,?,?,?,?)");

			pstmt.setString(1, user.getId());
			pstmt.setString(2, user.getCn());
			pstmt.setInt(3, 0);
			pstmt.setInt(4, 0);
			pstmt.setInt(5, 0);
			pstmt.executeUpdate();

		} catch (SQLException ex) {
			ex.printStackTrace();
		} finally {
			if (pstmt != null) {
				try {
					pstmt.close();
				} catch (SQLException ex) {
				}
			}
		}
	}

	public void UpdateUser(DBproperty user) {

		PreparedStatement pstmt = null;
		try {

			pstmt = conn
					.prepareStatement("update user set win = ? , lose = ?, score = ? where id = ?");
			pstmt.setInt(1, user.getWin());
			pstmt.setInt(2, user.getLose());
			pstmt.setInt(3, user.getScore());
			pstmt.setString(4, user.getId());
			pstmt.executeUpdate();
		} catch (SQLException ex) {
			ex.printStackTrace();
		} finally {
			if (pstmt != null) {
				try {
					pstmt.close();
				} catch (SQLException ex) {
				}
			}
		}
	}

	// ?±Î°ù???ÑÏù¥???∏Ï? Í≤?Ç¨
	public boolean FindUser(String username) {

		PreparedStatement pstmt = null;
		try {

			pstmt = conn
					.prepareStatement("select * from user where id like ?;");
			pstmt.setString(1, username);

		} catch (SQLException ex) {
			ex.printStackTrace();
		} finally {
			if (pstmt != null) {
				try {
					pstmt.close();
				} catch (SQLException ex) {
				}
			}
		}

		return false;
	}

	// CN?êÏÑú IDÍ∞íÏùÑ Î∞õÏïÑ??
	public DBproperty FindCn(String cn) {

		PreparedStatement pstmt = null;
		try {
			DBproperty setuser = new DBproperty();
			pstmt = conn
					.prepareStatement("select * from user where cn like ?;");
			pstmt.setString(1, cn);
			// System.out.

			ResultSet rs = pstmt.executeQuery();
			while (rs.next()) {
				String id = rs.getString(1);
				String usercn = rs.getString(2);
				int win = rs.getInt(3);
				int lose = rs.getInt(4);
				int score = rs.getInt(5);

				setuser.setId(id);
				setuser.setCn(usercn);
				setuser.setWin(win);
				setuser.setLose(lose);
				setuser.setScore(score);
			}
			return setuser;

		} catch (SQLException ex) {
			ex.printStackTrace();
		} finally {
			if (pstmt != null) {
				try {
					pstmt.close();
				} catch (SQLException ex) {
				}
			}
		}

		return null;
	}
	
	public DBproperty FindUserInfo(String user) {

		PreparedStatement pstmt = null;
		try {
			DBproperty setuser = new DBproperty();
			pstmt = conn
					.prepareStatement("select * from user where id like ?;");
			pstmt.setString(1, user);
			// System.out.

			ResultSet rs = pstmt.executeQuery();
			while (rs.next()) {
				String id = rs.getString(1);
				String usercn = rs.getString(2);
				int win = rs.getInt(3);
				int lose = rs.getInt(4);
				int score = rs.getInt(5);

				setuser.setId(id);
				setuser.setCn(usercn);
				setuser.setWin(win);
				setuser.setLose(lose);
				setuser.setScore(score);
			}
			return setuser;

		} catch (SQLException ex) {
			ex.printStackTrace();
		} finally {
			if (pstmt != null) {
				try {
					pstmt.close();
				} catch (SQLException ex) {
				}
			}
		}

		return null;
	}
}
