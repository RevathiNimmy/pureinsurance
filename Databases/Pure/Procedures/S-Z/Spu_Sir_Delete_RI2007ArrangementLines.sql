SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Sir_Delete_RI2007ArrangementLines'
GO

Create Procedure Spu_Sir_Delete_RI2007ArrangementLines
@Ri_Arrangement_Line_id int,
@Process_id int,
@Claim_id int,
@NewgroupingId int = null output  

As

Declare @newRiLineID as int
  
If @Process_id=1  
Begin  

     If Exists(Select * from Ri_Arrangement_Line where Grouping = @Ri_Arrangement_Line_id )
	Begin

	 Delete From Ri_Arrangement_Line where Ri_Arrangement_Line_id=@Ri_Arrangement_Line_id   

    	 Select @newRiLineID = MIN(Ri_Arrangement_Line_id) From Ri_Arrangement_Line
	 Where Grouping = @Ri_Arrangement_Line_id
		
	 Update Ri_Arrangement_Line Set Grouping = @newRiLineID 
	 Where Grouping = @Ri_Arrangement_Line_id

	 Set @NewgroupingId = @newRiLineID
	End
     Else
 	 Delete From Ri_Arrangement_Line where Ri_Arrangement_Line_id=@Ri_Arrangement_Line_id   
End  

If @Process_id=2  
Begin
     If Exists(Select * from Claim_Ri_Arrangement_Line where Grouping = @Ri_Arrangement_Line_id and Claim_id=@Claim_id  )
	Begin

	 Delete From Claim_Ri_Arrangement_Line where Ri_Arrangement_Line_id=@Ri_Arrangement_Line_id and Claim_id=@Claim_id    

    	 Select @newRiLineID = MIN(Ri_Arrangement_Line_id) From Claim_Ri_Arrangement_Line
	 Where Grouping = @Ri_Arrangement_Line_id and Claim_id=@Claim_id 
		
	 Update Claim_Ri_Arrangement_Line Set Grouping = @newRiLineID 
	 Where Grouping = @Ri_Arrangement_Line_id and Claim_id=@Claim_id 

	 Set @NewgroupingId = @newRiLineID
	End
     Else  
 	Delete From Claim_Ri_Arrangement_Line where Ri_Arrangement_Line_id=@Ri_Arrangement_Line_id and Claim_id=@Claim_id  
End  

Go