DDLDropview 'qryAllAccountDetails'
go

Create view [dbo].[qryAllAccountDetails] as
								Select 
								account_id								as account_id,
								pf.description							as PurgeFrequency,
								accstat.description						as AccountStatus,
								c.description							as Company,
								cur.description							as Currency,
								acctype.description						as AccountType,
								pt.description							as PaymentType,
								l.ledger_name							as Ledger,
								a.account_name,
								a.short_code,
								a.contact_name,
								a.address1,
								a.address2,
								a.address3,
								a.address4,
								a.postal_code,
								addcountry.description					as AddressCountry,
								a.phone_area_code,
								a.phone_number,
								a.phone_extension,
								a.fax_area_code, 
								a.fax_number,
								a.fax_extension,
								a.payment_name,
								a.payment_account_code,
								a.payment_branch_code,
								convert(datetime, convert(varchar(23), 
								   a.payment_expiry_date, 111))			as payment_expiry_date,
								a.payment_reference1,
								a.payment_reference2,
								a.credit_limit,
								a.discount_percentage,
								a.settlement_period bank_name,
								a.bank_address1,
								a.bank_address2,
								a.bank_address3,
								a.bank_address4,
								a.bank_postal_code,
								bankcountry.description					as BankCountry,
								a.bank_phone_area_code,
								a.bank_phone_number,
								a.bank_phone_extension,
								a.bank_fax_area_code,
								a.bank_fax_number,
								a.bank_fax_extension,
								a.comments,
								a.account_key,
								a.nominal_account_id,
								a.prooflist_report_id,
								a.bordereau_report_id,
								a.allow_electronic_payment,
								a.client_money_calc_account_type,
								a.client_bank_account_type
								from account a 
								left join purgefrequency pf on a.purgefrequency_id = pf.purgefrequency_id
								left join accountstatus accstat on a.accountstatus_id = accstat.accountstatus_id
								left join company c on a.company_id = c.company_id
								left join currency cur on a.currency_id = cur.currency_id
								left join accounttype acctype on a.accounttype_id = acctype.accounttype_id
								left join paymenttype pt on a.paymenttype_id = pt.paymenttype_id
								left join ledger l on a.ledger_id= l.ledger_id
								left join country addcountry on a.address_country = addcountry.country_id
								left join country bankcountry on a.bank_country = bankcountry.country_id