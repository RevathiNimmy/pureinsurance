SET QUOTED_IDENTIFIER OFF
Go
SET ANSI_NULLS OFF  
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_GIS_Output'
GO

CREATE PROCEDURE spu_SIR_Get_GIS_Output
    @PolicyBinderId int,
    @DataModel varchar(10),
    @RoundingSectionCode varchar(10) = NULL
AS
BEGIN

DECLARE @SQLString NVARCHAR(3000)

Set @SQLString = 'SELECT O.' + @DataModel + '_Policy_binder_id,' + CHAR(13)
Set @SQLString = @SQLString + 'O.' + @DataModel + '_Output_id,' + CHAR(13)
Set @SQLString = @SQLString + 'O.Decline_reason, O.Refer_reason, O.Message, O.policy_rating_section,' + CHAR(13)
Set @SQLString = @SQLString + 'O.risk_rating_section, ROUND(O.sum_insured,4), Round(O.premium,4), O.rate, 0 original_premium,' + CHAR(13)
Set @SQLString = @SQLString + '0 flag, O.rate_type_id, O.country_id, O.state_id, 1, EPU.earning_pattern_id, o.Disable_Original_ProRata, o.Disable_New_ProRata,' + CHAR(13)
Set @SQLString = @SQLString + '0' + CHAR(13)
Set @SQLString = @SQLString + 'FROM ' + @DataModel + '_Policy_binder PB' + CHAR(13)
Set @SQLString = @SQLString + 'INNER JOIN ' + @DataModel + '_Output O ON O.' + @DataModel + '_Policy_binder_id = PB.' + @DataModel  + '_Policy_binder_id' + CHAR(13)
Set @SQLString = @SQLString + 'LEFT JOIN rating_section_type RST ON RST.code = O.risk_rating_section' + CHAR(13)
Set @SQLString = @SQLString + 'LEFT JOIN (Select * from earning_pattern_usage where earning_pattern_usage_id IN' + CHAR(13)
Set @SQLString = @SQLString + '(Select Max(earning_pattern_usage_id) From earning_pattern_usage EPU1' + CHAR(13)
Set @SQLString = @SQLString + 'Group By rating_section_type_id)) EPU' + CHAR(13)
Set @SQLString = @SQLString + 'ON RST.rating_section_type_id = EPU.rating_section_type_id' + CHAR(13)
Set @SQLString = @SQLString + 'WHERE PB.gis_policy_link_id = ' + CAST(@PolicyBinderId AS Varchar) + CHAR(13)

IF (RTrim(ISNULL(@RoundingSectionCode, '')) <> '')
    Set @SQLString = @SQLString + 'AND ISNULL(O.risk_rating_section, '''') <> ''' + RTrim(@RoundingSectionCode) + '''' + CHAR(13)

Set @SQLString = @SQLString + 'ORDER BY O.' + @DataModel + '_Output_id' 

EXEC sp_executesql @SQLString

END
