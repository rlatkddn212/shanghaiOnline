package prodconsumselectserver;
//DB???¬ìš©???„ë¡œ?¼í‹°
/***
 * 
 * @author rlatkddn212
 *
 */
public class DBproperty {
	private String id;
	private String cn;
	private int win;
	private int lose;
	private int score;
	
	
	public void setId(String id)
	{
		this.id = id; 
	}
	
	public String getId()
	{
		return id;
	}
	public void setCn(String cn)
	{
		this.cn = cn; 
	}
	
	public String getCn()
	{
		return cn;
	}
	public void setWin(int win)
	{
		this.win = win; 
	}
	
	public int getWin()
	{
		return win;
	}
	public void setLose(int lose)
	{
		this.lose = lose; 
	}
	
	public int getLose()
	{
		return lose;
	}
	public void setScore(int score)
	{
		this.score = score; 
	}
	
	public int getScore()
	{
		return score;
	}
}
