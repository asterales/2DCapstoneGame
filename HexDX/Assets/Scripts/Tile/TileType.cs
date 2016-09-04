public enum TileType {
	Greenery = 0  // maybe set 0 to None ? 
}

// One way of associating enums with other information. The alternative way is using custom C# attributes which are a little more involved
public static class TileTypeExtensions {
	public static bool IsPathable (this TileType self){
		switch(self){
			case TileType.Greenery:
				return true;
			default:
				return false;
		}
	}
}