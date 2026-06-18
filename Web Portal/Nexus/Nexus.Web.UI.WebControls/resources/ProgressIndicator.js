function SetUpdateIndicatorLocation(upPanelId, upDivId, cssClass) {
    // get the update panel element and indicator div controls 
    var upPanel = document.getElementById(upPanelId);
    var upDiv = document.getElementById(upDivId);
    if (upDiv != null) {
        if (IsBlockUIEnabled == "true") {
            upDiv.style.display = "none";
        } else {
            //set the update div to the same location as the update panel via CSS 
            if (upDiv != null) {
                if (IsBlockUIEnabled == "true") {
                    upDiv.style.display = "none";
                }
                else {
                    upDiv.style.width = upPanel.clientWidth + 'px';
                    upDiv.style.height = upPanel.clientHeight + 'px';
                    upDiv.style.left = upPanel.offsetLeft + 'px';
                    upDiv.style.top = upPanel.offsetTop + 'px';
                    upDiv.style.position = "absolute";
                    upDiv.className = cssClass;
                }
            }
            return true;
        }
    }
}
