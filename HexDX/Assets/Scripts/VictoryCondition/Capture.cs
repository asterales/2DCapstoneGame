public class Capture : VictoryCondition {
    public int row;
    public int col;
    private Tile goal; 

	public override void Init() {
		goal = HexMap.mapArray[row][col];
	}

    public override bool Achieved() {
        return goal.currentUnit != null && goal.currentUnit.IsPlayerUnit();
    }
}