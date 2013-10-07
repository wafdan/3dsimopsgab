    // For dropdownselect
    var showDropdown= false;
     
    var dropdownGridInt : int = 0;
    private var dropdownStrings : String[] = ["drop1", "drop 2","drop3", "drop 4","drop5", "drop 6", "drop 7", "drop 8", "drop 9", "drop 10"];
     
    private var scrollViewVector : Vector2 = Vector2.zero;
    private var lastSelection = 0;
     
    function OnGUI () {
       if(GUI.Button (Rect(10, Screen.height/7, Screen.width/8, Screen.height/20), "Information")) {
          showDropdown = !showDropdown;
          lastSelection = dropdownGridInt;
       }
       if(showDropdown){
          //code for Scrollview & SelectionGrid
          scrollViewVector=GUI.BeginScrollView (Rect (10, Screen.height/5, Screen.width/8, Screen.height/14), scrollViewVector, Rect (0, 0, Screen.width/12, 300),false,true);
             dropdownGridInt= GUI.SelectionGrid (Rect (0, 0, Screen.width/10, Screen.height/3), dropdownGridInt, dropdownStrings, 1);
             if(dropdownGridInt!=lastSelection) showDropdown=true;
          GUI.EndScrollView();
       }
    }