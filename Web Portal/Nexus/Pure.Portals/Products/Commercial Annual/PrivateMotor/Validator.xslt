<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes"/>

	<xsl:template match="//DATA_SET">
		<ValidationFailures>
			<xsl:apply-templates select="//DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER" mode="control"></xsl:apply-templates>
		</ValidationFailures>
	</xsl:template>

	<xsl:template mode="control" match="//DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER">
		<xsl:apply-templates select="//DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER/VEHDET" mode="validation" />
		<xsl:apply-templates select="//DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER/PREVINSURER" mode="validation" />
	</xsl:template>

	<xsl:template name="suminsured" mode="validation" match="//DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER/VEHDET">
		<xsl:if test="not(@SI) or (@SI='normalize-space(.)')">
			<ValidationFailure>Sum Insured is mandatory (Vehicle Details tab)</ValidationFailure>
		</xsl:if>
	</xsl:template>

	<xsl:template name="previnsurer" mode="validation" match="//DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER/PREVINSURER">
		<xsl:if test="not(@PREVPOLNO) or (@PREVPOLNO='normalize-space(.)')">
			<ValidationFailure>Previous Policy Number is mandatory (Travel Detials tab)</ValidationFailure>
		</xsl:if>
	</xsl:template>
	

</xsl:stylesheet>