var myLeft:float = 10;
var myTop:float = 30;
var myWidth:float = 50;
var myHeight:float = 50;
     
var isDragging:boolean = false;
     
function OnGUI()
{
    if ( GUI.RepeatButton( Rect(myLeft, myTop, myWidth, myHeight) , "unit" ) )
    isDragging = true;
     
    if (isDragging)
    {
	    myLeft = Input.mousePosition.x - myWidth*0.5;
    	myTop = Screen.height - (Input.mousePosition.y + myHeight*0.5);
    }
}
     
function Update()
{
    if ( Input.GetMouseButtonUp(0) )
    isDragging = false;
     
}