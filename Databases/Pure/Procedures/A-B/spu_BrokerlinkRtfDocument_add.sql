ddldropprocedure spu_BrokerlinkRtfDocument_add
go

CREATE PROCEDURE spu_BrokerlinkRtfDocument_add
   @Gis_Policy_Link_Id integer,
   @Brokerlink_Document_Type_Id integer,
   @Contents_Rtf text
AS
if exists(select null from Brokerlink_Rtf_Document where Gis_Policy_Link_Id = @Gis_Policy_Link_Id and 
		Brokerlink_Document_Type_Id = @Brokerlink_Document_Type_Id)
Begin
	update Brokerlink_Rtf_Document set Contents_Rtf = @Contents_Rtf where 
		Gis_Policy_Link_Id = @Gis_Policy_Link_Id and 
		Brokerlink_Document_Type_Id = @Brokerlink_Document_Type_Id
End
Else
Begin
	insert into Brokerlink_Rtf_Document(Gis_Policy_Link_Id, Brokerlink_Document_Type_Id, Contents_Rtf)
	values(@Gis_Policy_Link_Id,@Brokerlink_Document_Type_Id,@Contents_Rtf)
End
go