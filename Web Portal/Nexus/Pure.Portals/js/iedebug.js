var plModal = null;
$(document).ready(function(){

	$("input[alt*=PickListContainer]").click(function(){
		plModal = $("#PickListContainer").html();
		$("#PickListContainer").html("");
		$("#TB_window").html(plModal);
		return false;
	});
});