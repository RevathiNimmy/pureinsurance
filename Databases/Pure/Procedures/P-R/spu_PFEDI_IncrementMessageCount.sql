DDLDropProcedure 'spu_PFEDI_IncrementMessageCount'
GO

CREATE PROCEDURE spu_PFEDI_IncrementMessageCount 
(
	@CompanyNo INT,
	@SchemeNo INT,
	@SchemeVersion INT
)

AS

declare @party_cnt as integer				           --PN12383
select @party_cnt = (select party_cnt  
		     from pfscheme
		     where schemeno = @SchemeNo
                     and CompanyNo=@CompanyNo	                   --PN12383
		     and schemeversion = @SchemeVersion)           --PN14910

UPDATE PFScheme
SET EDIMessageCount=  (SELECT MAX(ISNULL(EDIMessageCount,0))+1     --PN12383 Get next edi number 
				FROM PFScheme			   --for Provider
				WHERE party_cnt = @party_cnt
			  )				 
WHERE
	CompanyNo=@CompanyNo
AND	SchemeNo=@SchemeNo
AND	SchemeVersion=@SchemeVersion
GO
