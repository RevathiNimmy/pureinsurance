SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


ALTER    PROCEDURE spu_GIS_Get_List_Group_Items
            @gis_scheme_id      int,
            @gis_list_type_id   int,
            @gis_list_grouping_id   int
AS
BEGIN

    /*
        Procedure: sp_GIS_Get_List_Group_Items

        History : 20/11/2001 CTAF Created
	          10/12/2001 CTAF Added description column and changed everything to dynamic sql.
                  	     Side note: I hate this way of doing things with dynamic tables. It's rubbish!
		  03/02/2003 APS  Added clause of gis_list_items_id when selecting from type usage table
    */

DECLARE @sSQL1 varchar(255)
DECLARE @sSQL2 varchar(300)
DECLARE @sSQL3 varchar(255)
DECLARE @sSQL4 varchar(255)
DECLARE @sSQL5 varchar(255)
DECLARE @sSQL6 varchar(255)
DECLARE @sSQL7 varchar(255)
DECLARE @sSQL8 varchar(255)
DECLARE @sSQL9 varchar(255)
DECLARE @sSQLA varchar(255)
DECLARE @sSQLB varchar(255)
DECLARE @sUTable varchar(255)
DECLARE @sType varchar(255)

-- Variables to convert from
DECLARE @sGISSchemeID varchar(10)
DECLARE @sGISListTypeID varchar(10)
DECLARE @sGISListGroupingID varchar(10)

    -- Convert the variables
    SELECT @sGISSchemeID = CONVERT(varchar(10),@gis_scheme_id)
    SELECT @sGISListTypeID = CONVERT(varchar(10),@gis_list_type_id)
    SELECT @sGISListGroupingID = CONVERT(varchar(10),@gis_list_grouping_id)

    -- Get the type code
    SELECT  @sType = code
    FROM    gis_list_type
    WHERE   gis_list_type_id = @gis_list_type_id

    -- Get the name of the UDL_Table
    SELECT  @sUTable = 'UDL_' + RTRIM(@sType)

    IF (@gis_list_grouping_id <> 0)
    BEGIN

        -- Get the items that are being used
        SELECT @sSQL1 = ''
        SELECT @sSQL1 = @sSQL1 + 'SELECT gli.[gis_list_items_id], '
        SELECT @sSQL1 = @sSQL1 + 'gli.[code], '
        SELECT @sSQL1 = @sSQL1 + 'ut.[description], '
        SELECT @sSQL1 = @sSQL1 + '1 as ''is_selected'' '
        SELECT @sSQL1 = @sSQL1 + 'FROM gis_list_items gli '
        SELECT @sSQL1 = @sSQL1 + 'INNER JOIN ' + @sUTable + ' ut '

        SELECT @sSQL2 = ''
        SELECT @sSQL2 = @sSQL2 + 'ON ut.[code] = gli.[code] '
        SELECT @sSQL2 = @sSQL2 + 'INNER JOIN gis_list_grouping_items glgi '
        SELECT @sSQL2 = @sSQL2 + 'ON glgi.[gis_list_items_id] = gli.[gis_list_items_id] '
        SELECT @sSQL2 = @sSQL2 + 'INNER JOIN gis_list_grouping glg '

        SELECT @sSQL3 = ''
        SELECT @sSQL3 = @sSQL3 + 'ON glg.[gis_list_grouping_id] = glgi.[gis_list_grouping_id] '
        SELECT @sSQL3 = @sSQL3 + 'WHERE glgi.[gis_scheme_id] = ' + @sGISSchemeID + ' '

        SELECT @sSQL4 = ''
        SELECT @sSQL4 = @sSQL4 + 'AND glg.[gis_list_type_id] = ' + @sGISListTypeID + ' '
        SELECT @sSQL4 = @sSQL4 + 'AND glgi.[gis_list_grouping_id] = ' + @sGISListGroupingID + ' '

        SELECT @sSQL4 = @sSQL4 + ' UNION '

        -- Get the items that aren't being used
        SELECT @sSQL5 = ''
        SELECT @sSQL5 = @sSQL5 + 'SELECT gli.[gis_list_items_id], '
        SELECT @sSQL5 = @sSQL5 + 'gli.[code], '
        SELECT @sSQL5 = @sSQL5 + 'ut.[description], '
        SELECT @sSQL5 = @sSQL5 + '0 as ''is_selected'' '
        SELECT @sSQL5 = @sSQL5 + 'FROM gis_list_items gli '
        SELECT @sSQL5 = @sSQL5 + 'INNER JOIN ' + @sUTable + ' ut '

        SELECT @sSQL6 = ''
        SELECT @sSQL6 = @sSQL6 + 'ON ut.[code] = gli.[code] '
        SELECT @sSQL6 = @sSQL6 + 'INNER JOIN gis_list_type_usage gltu '
        SELECT @sSQL6 = @sSQL6 + 'ON gltu.[gis_list_items_id] = gli.[gis_list_items_id] '
        SELECT @sSQL6 = @sSQL6 + 'WHERE gltu.[gis_list_type_id] = ' + @sGISListTypeID + ' '
        SELECT @sSQL6 = @sSQL6 + 'AND gltu.[gis_list_items_id] NOT IN '
        SELECT @sSQL6 = @sSQL6 + '('

        SELECT @sSQL7 = ''
        SELECT @sSQL7 = @sSQL7 + 'SELECT gli.[gis_list_items_id] '
        SELECT @sSQL7 = @sSQL7 + 'FROM gis_list_items gli '
        SELECT @sSQL7 = @sSQL7 + 'INNER JOIN gis_list_grouping_items glgi '
        SELECT @sSQL7 = @sSQL7 + 'ON glgi.[gis_list_items_id] = gli.[gis_list_items_id] '
        SELECT @sSQL7 = @sSQL7 + 'INNER JOIN gis_list_grouping glg '
        SELECT @sSQL7 = @sSQL7 + 'ON glg.[gis_list_grouping_id] = glgi.[gis_list_grouping_id] '

        SELECT @sSQL8 = ''      
        SELECT @sSQL8 = @sSQL8 + 'WHERE glgi.[gis_scheme_id] = ' + @sGISSchemeID + ' '
        SELECT @sSQL8 = @sSQL8 + 'AND glg.[gis_list_type_id] = ' + @sGISListTypeID + ' '
        SELECT @sSQL8 = @sSQL8 + 'AND glgi.[gis_list_grouping_id] = ' + @sGISListGroupingID + ' '
        SELECT @sSQL8 = @sSQL8 + ') '

        SELECT @sSQL9 = ''
	SELECT @sSQL9 = @sSQL9 + 'AND gltu.[version] = (SELECT MAX(gltu.[version]) '
	SELECT @sSQL9 = @sSQL9 + 'FROM gis_list_type_usage gltu '
	SELECT @sSQL9 = @sSQL9 + 'WHERE gltu.[gis_list_type_id] = ' + @sGISListTypeID + ' '
	SELECT @sSQL9 = @sSQL9 + 'AND gltu.gis_list_items_id = gli.[gis_list_items_id] ) '

        --DECLARE @sTemp varchar(255)
        --SELECT @sTemp = MAX(gltu.[version]) FROM gis_list_type_usage gltu WHERE gltu.[gis_list_type_id] = @sGISListTypeID
        --SELECT @sSQL9 = @sSQL9 +  'AND gltu.[version] = ' + @sTemp

        /* Filter out the ones in use */
        SELECT @sSQLA = ''
        SELECT @sSQLA = @sSQLA + 'AND gli.[gis_list_items_id] NOT IN ('
        SELECT @sSQLA = @sSQLA + 'SELECT glgi.[gis_list_items_id] FROM gis_list_grouping_items glgi WHERE glgi.[gis_list_grouping_id] IN '

        SELECT @sSQLB = ''
        SELECT @sSQLB = @sSQLB + '(SELECT glg.[gis_list_grouping_id] FROM gis_list_grouping glg WHERE glg.[gis_list_type_id] = ' + @sGISListTypeID + ' and glg.[gis_scheme_id] = ' + @sGISSchemeID + '))'

        -- PRINT (@sSQL1+@sSQL2+@sSQL3+@sSQL4+@sSQL5+@sSQL6+@sSQL7+@sSQL8+@sSQL9+@sSQLA+@sSQLB)

	EXECUTE (@sSQL1+@sSQL2+@sSQL3+@sSQL4+@sSQL5+@sSQL6+@sSQL7+@sSQL8+@sSQL9+@sSQLA+@sSQLB)

    END
    ELSE
    BEGIN

        -- Get the items that aren't being used
        SELECT @sSQL1 = ''
        SELECT @sSQL1 = @sSQL1 + 'SELECT gli.[gis_list_items_id],'
        SELECT @sSQL1 = @sSQL1 + 'gli.[code],'
        SELECT @sSQL1 = @sSQL1 + 'ut.[description],'
        SELECT @sSQL1 = @sSQL1 + '0 as ''is_selected'''
        SELECT @sSQL2 = ''
        SELECT @sSQL2 = @sSQL2 + 'FROM gis_list_items gli '
        SELECT @sSQL2 = @sSQL2 + 'INNER JOIN gis_list_type_usage gltu '
        SELECT @sSQL2 = @sSQL2 + 'ON gltu.[gis_list_items_id] = gli.[gis_list_items_id] '
        SELECT @sSQL2 = @sSQL2 + ' AND gltu.[version] = (SELECT MAX(version) '
        SELECT @sSQL2 = @sSQL2 + ' FROM gis_list_type_usage '
        SELECT @sSQL2 = @sSQL2 + ' WHERE gis_list_type_id = gltu.[gis_list_type_id]'
        SELECT @sSQL2 = @sSQL2 + ' AND  gis_list_items_id = gli.[gis_list_items_id]) '


        SELECT @sSQL3 = ''
        SELECT @sSQL3 = @sSQL3 + 'INNER JOIN ' + @sUTable + ' ut '
        SELECT @sSQL3 = @sSQL3 + 'ON ut.[code] = gli.[code] '
        SELECT @sSQL3 = @sSQL3 + 'WHERE gltu.[gis_list_type_id] = ' + @sGISListTypeID

        SELECT @sSQLA = ''
        SELECT @sSQLA = @sSQLA + 'AND gli.[gis_list_items_id] NOT IN ('
        SELECT @sSQLA = @sSQLA + 'SELECT glgi.[gis_list_items_id] FROM gis_list_grouping_items glgi WHERE glgi.[gis_list_grouping_id] IN '

        SELECT @sSQLB = ''
        SELECT @sSQLB = @sSQLB + '(SELECT glg.[gis_list_grouping_id] FROM gis_list_grouping glg WHERE glg.[gis_list_type_id] = ' + @sGISListTypeID + ' and glg.[gis_scheme_id] = ' + @sGISSchemeID + '))'

        PRINT (@sSQL1 + @sSQL2 + @sSQL3 + @sSQLA + @sSQLB)

        EXECUTE (@sSQL1 + @sSQL2 + @sSQL3 + @sSQLA + @sSQLB)

    END

END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

