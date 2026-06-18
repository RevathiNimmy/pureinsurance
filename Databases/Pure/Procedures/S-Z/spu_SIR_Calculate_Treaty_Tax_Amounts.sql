SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_SIR_Calculate_Treaty_Tax_Amounts'
GO


CREATE PROCEDURE spu_SIR_Calculate_Treaty_Tax_Amounts    
    @insurance_file_cnt int, 
    @risk_cnt int,
    @ri_arrangement_line_id int,
    @treaty_id int, 
    @premium money, 
    @commission money, 
    @premium_transtype varchar(20), 
    @commission_transtype varchar(20),
    @premium_tax money output, 
    @commission_tax money output
As

    Declare @party_cnt int,
            @share_percent money, 
            @party_premium money, 
            @party_commission money, 
            @party_premium_tax money, 
            @party_commission_tax money
    
    -- Default
    Select  @premium_tax = 0,
            @commission_tax = 0 

    -- Declare cursor to process all parties in the treaty    
    DECLARE CURSOR_Treaty_Party CURSOR FAST_FORWARD FOR     
        SELECT  party_cnt, 
                share_percent  
        FROM    treaty_party
        WHERE   treaty_id = @treaty_id
    
    OPEN CURSOR_Treaty_Party    
    FETCH NEXT FROM CURSOR_Treaty_Party INTO     
        @party_cnt, 
        @share_percent
     
    WHILE @@FETCH_STATUS = 0 BEGIN    
        -- get the share of premium and commission value based on the share percentage 
        -- of the treaty party that should used in the tax calculations  
        Select  @party_premium = ((@premium * @share_percent) / 100),
                @party_commission = ((@commission * @share_percent) / 100)
        
        -- calculate tax amounts for treaty party  
        Execute spu_SIR_Calculate_Treaty_Party_Tax_Amounts    
            @insurance_file_cnt, 
            @risk_cnt, 
            @ri_arrangement_line_id,
            @party_cnt, 
            @party_premium, 
            @party_commission, 
            @premium_transtype, 
            @commission_transtype, 
            @party_premium_tax output, 
            @party_commission_tax output
 
        -- get totals for tax amounts (withholding has already been taken into account)
        Select  @premium_tax = @premium_tax + @party_premium_tax,
                @commission_tax = @commission_tax + @party_commission_tax  

        -- Get next        
        FETCH NEXT FROM CURSOR_Treaty_Party INTO     
            @party_cnt, 
            @share_percent
    END   
    
    CLOSE CURSOR_Treaty_Party    
    DEALLOCATE CURSOR_Treaty_Party    
    

GO


