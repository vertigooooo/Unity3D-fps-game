
public class GameData {
    public enum GameResult
    {
        win,
        lose,
        time_out,
        not_sure
    };
    //游戏结果
    public static GameResult gameResult = GameResult.not_sure;
    //游戏难度
    public static int level = 1;
}
