if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_gis_clear_quote_output]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_gis_clear_quote_output]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE spu_gis_clear_quote_output

--deletes output details from every output table ready for re-run of rules

@policylinkID int,
@datamodelcode nvarchar(30),
@schemename nvarchar(50) = NULL,
@sKeep nvarchar(100) = NULL

as

set quoted_identifier on

declare @OD nvarchar(30)
declare @ODid nvarchar(30)
declare @OE nvarchar(30)
declare @OEid nvarchar(30)
declare @ON nvarchar(30)
declare @ONid nvarchar(30)
declare @OFee nvarchar(30)
declare @OFeeId nvarchar(30)
declare @O nvarchar(30)
declare @Oid nvarchar(30)
declare @PB nvarchar(30)
declare @PBid nvarchar(30)
declare @SQL nvarchar(500)
declare @MaxOutputID int
declare @PolicyBinderID int
declare @ODString varchar(10)
declare @PBString varchar(10)
declare @PLID varchar(10)
declare @CommissionId nvarchar(30)

select @OD = @datamodelcode + '_output_details'
select @ODid = @datamodelcode + '_output_details_id'
select @OE = @datamodelcode + '_output_excess'
select @OEid = @datamodelcode + '_output_excess_id'
select @ON = @datamodelcode + '_output_endorsements'
select @ONid = @datamodelcode + '_output_endorsements_id'
select @OFee = @datamodelcode + '_output_fees'
select @OFeeId = @datamodelcode + '_output_fees_id'
select @O = @datamodelcode + '_output'
select @Oid = @datamodelcode + '_output_id'
select @PB= @datamodelcode + '_policy_binder'
select @PBid = @datamodelcode + '_policy_binder_id'
select @CommissionId =  @datamodelcode + '_output_commission_id'
--build sql

select @PLID = cast(@policyLinkID as varchar(10))

-- CTAF 20020711 Named the return columns for readability
IF ISNULL(@sKeep, '') = ''
BEGIN
	select @SQL = ' select o.' + @OID + ' as max_output_id, b.'+@PBID+' as policy_binder_id from '+ @O +' o 
	 inner join '+@PB+'  b on o.'+@PBID+'= b.'+@PBID+' where b.gis_policy_link_id=' +@PLID 
END
ELSE
BEGIN
	select @SQL = ' select o.' + @OID + ' as max_output_id, b.'+@PBID+' as policy_binder_id from '+ @O +' o 
	 inner join '+@PB+'  b on o.'+@PBID+'= b.'+@PBID+' where b.gis_policy_link_id=' +@PLID + '   and o.' + @OID + ' NOT IN (' + @sKeep + ')'
END 

-- CTAF 180402 Add scheme_desc if passed in
IF @schemename IS NOT NULL
BEGIN
	SELECT @SQL = @SQL + '   and o.scheme_desc='''  + @schemename + ''''
END 

-- CTAF 20020711 Begin
-- Use a (slow) temp table instead of a cursor
-- If this is TOO slow then use a perm table with a user/session id on the end of it
CREATE TABLE #ClearOutputTemp
(
	max_output_id int,
	policy_binder_id int
)

PRINT @SQL

INSERT INTO #ClearOutputTemp EXECUTE (@SQL)

--get max output id, policy binder id
--exec ('DECLARE bob CURSOR FOR '  + @SQL )
-- CTAF - Took the liberty of renaming it from bob to something better...
DECLARE cClear CURSOR FOR SELECT max_output_id, policy_binder_id FROM #ClearOutputTemp

-- CTAF 20020711 End

OPEN cClear 

FETCH NEXT FROM cClear INTO @MaxOutputID,@PolicyBinderID

WHILE @@fetch_status=0
BEGIN

print @Maxoutputid
print @policybinderid

--convert
SELECT @ODString=cast(@maxoutputid as varchar(10))
SELECT @PBString=cast(@policybinderid as varchar(10))

-- CTAF 20020711 Changed the following to use sp_executesql

--output details
SELECT @SQL = 'DELETE  ' + @OD + ' where ' + @OID + ' = ' + @ODString + ' and '  +  @PBID + ' = ' + @PBString
EXECUTE sp_executesql @SQL

--output excess
SELECT @SQL = N'DELETE  ' + @OE + ' where ' + @OID + ' = ' + @ODString + ' and '  +  @PBID + ' = ' + @PBString
EXECUTE sp_executesql @SQL

--output endorsements
SELECT @SQL = N'DELETE  ' + @ON + ' where ' + @OID + ' = ' + @ODString + ' and '  +  @PBID + ' = ' + @PBString
EXECUTE sp_executesql @SQL

--output fees
SELECT @SQL = N'DELETE  ' + @OFee + ' where ' + @OID + ' = ' + @ODString + ' and '  +  @PBID + ' = ' + @PBString
EXECUTE sp_executesql @SQL

--output
SELECT @SQL = N'DELETE  ' + @O + ' where ' + @OID + ' = ' + @ODString + ' and '  +  @PBID + ' = ' + @PBString
EXECUTE sp_executesql @SQL

--output commission
SELECT @SQL = N'DELETE ' + @CommissionId + ' where ' + @OID + ' = ' + @ODString + ' and ' + @PBID + ' = ' + @PBString 
EXECUTE sp_executesql @SQL

FETCH NEXT FROM cClear into @MaxOutputID,@PolicyBinderID

END

CLOSE cClear
DEALLOCATE cClear

-- CTAF 20020711 Begin
DROP TABLE #ClearOutputTemp 
-- CTAF 20020711 End
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

