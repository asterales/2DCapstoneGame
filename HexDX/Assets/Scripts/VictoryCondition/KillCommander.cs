public class KillCommander: VictoryCondition {
	// might need a better way of identifying unit to capture
    public int row;
    public int col;
    private Unit commander;

    public override void Init() { 
    	commander = HexMap.mapArray[row][col].currentUnit;
    }

    public override bool Achieved() {
        return !commander.gameObject.activeSelf; 
    }
}