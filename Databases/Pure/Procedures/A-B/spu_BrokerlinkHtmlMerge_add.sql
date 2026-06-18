ddldropprocedure spu_BrokerlinkHtmlMerge_add
go

CREATE PROCEDURE spu_BrokerlinkHtmlMerge_add
   @Gis_Policy_Link_Id integer,
   @Brokerlink_Document_Type_Id integer,
   @text_html text
AS
if exists(select null from Brokerlink_Html_Merge where Gis_Policy_Link_Id = @Gis_Policy_Link_Id and 
		Brokerlink_Document_Type_Id = @Brokerlink_Document_Type_Id)
Begin
	update Brokerlink_Html_Merge set text_html = @text_html where 
		Gis_Policy_Link_Id = @Gis_Policy_Link_Id and 
		Brokerlink_Document_Type_Id = @Brokerlink_Document_Type_Id
End
Else
Begin
	insert into Brokerlink_Html_Merge(Gis_Policy_Link_Id, Brokerlink_Document_Type_Id, text_html)
	values(@Gis_Policy_Link_Id,@Brokerlink_Document_Type_Id,@text_html)
End
go

