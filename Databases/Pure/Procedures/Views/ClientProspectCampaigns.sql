set quoted_identifier on set ansi_nulls on
go

execute DDLDropView 'ClientProspectCampaigns'
go
create view ClientProspectCampaigns as select
    Prospect_Campaign.party_cnt     ClientID,
    Campaign.code                   Code,
    Campaign.description            Description,
    Campaign.campaign_date          CampaignDate
    from Prospect_Campaign
    inner join Campaign on Prospect_Campaign.campaign_id = Campaign.campaign_id
go
