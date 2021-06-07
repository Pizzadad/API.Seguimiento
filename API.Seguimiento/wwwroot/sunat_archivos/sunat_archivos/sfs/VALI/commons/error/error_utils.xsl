<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	 xmlns:json="http://www.ibm.com/xmlns/prod/2009/jsonx">
<xsl:output method="xml" omit-xml-declaration="yes"/>
	<xsl:template name="rejectCall">
		<xsl:param name="errorCode" />
		<xsl:param name="errorMessage" />
		<xsl:param name="priority" select="'error'"/>
		
        <xsl:call-template name="addResult">
            <xsl:with-param name="isError" select="true()" />
            <xsl:with-param name="code" select="$errorCode" />
            <xsl:with-param name="message" select="$errorMessage" />
        </xsl:call-template>

	</xsl:template>
	
	
	

	<xsl:template name="error">
		<xsl:param name="codigo" />
		<!-- <xsl:variable name="descripcionError" select="document('local:///commo_ns/cpe/catalogo/CatalogoErrores.xml')" /> -->
		<xsl:variable name="descripcionError" select="document('../../../VALI/commons/cpe/catalogo/CatalogoErrores.xml')" />
		<xsl:value-of select="$descripcionError/catalogoerrores/error[@numero=$codigo]" />
	</xsl:template>
	
	<!-- genera mensaje de warning -->
	<xsl:template name="addWarning">
		<xsl:param name="warningCode" />
		<xsl:param name="warningMessage" />
		
        <xsl:call-template name="addResult">
            <xsl:with-param name="isError" select="false()" />
            <xsl:with-param name="code" select="$warningCode" />
            <xsl:with-param name="message" select="$warningMessage" />
        </xsl:call-template>

	</xsl:template>
	
	<!-- >Giansalex Custom templates -->
	<xsl:template name="addResultWithNode">
        <xsl:param name="isError" />
        <xsl:param name="code" />
        <xsl:param name="message" />
        <xsl:param name="node" />

        <xsl:variable name="path">
            <xsl:call-template name="getPath">
                <xsl:with-param name="node" select="$node" />
            </xsl:call-template>
        </xsl:variable>

        <xsl:call-template name="addResult">
            <xsl:with-param name="isError" select="$isError" />
            <xsl:with-param name="code" select="$code"/>
            <xsl:with-param name="message" select="concat($message, '|', $path, '|', $node)"/>
        </xsl:call-template>
    </xsl:template>

    <xsl:template name="addResult">
        <xsl:param name="isError" />
        <xsl:param name="code" />
        <xsl:param name="message" />

        <xsl:variable name="level">
            <xsl:choose>
                <xsl:when test="$isError">
                    <xsl:value-of select="1" />
                </xsl:when>
                <xsl:otherwise>
                    <xsl:value-of select="2" />
                </xsl:otherwise>
            </xsl:choose>
        </xsl:variable>

        <xsl:variable name="result">
            <xsl:value-of select="concat($level, '|', $code, '|', $message)" />
        </xsl:variable>

        <xsl:choose>
            <xsl:when test="$isError">
                <!-- Terminate Validation -->
                <xsl:message terminate="yes">
                    <xsl:value-of select="$result" />
                </xsl:message>
            </xsl:when>
            <xsl:otherwise>
                <xsl:message terminate="yes">
                    <xsl:value-of select="$result" />
                </xsl:message>
            </xsl:otherwise>
        </xsl:choose>
    </xsl:template>

    <xsl:template name="getPath">
        <xsl:param name="node" />
        <xsl:for-each select="$node/parent::*">
            <xsl:call-template name="getPath">
                <xsl:with-param name="node" select="current()"/>
            </xsl:call-template>
        </xsl:for-each>
        <xsl:choose>
            <xsl:when test="$node/self::*">
                <xsl:text>/</xsl:text>
            </xsl:when>
            <xsl:when test="count($node|$node/../@*)=count($node/../@*)">
                <xsl:text>@</xsl:text>
            </xsl:when>
        </xsl:choose>
        <xsl:value-of select="name($node)"/>
    </xsl:template>
	<!-- <Giansalex Custom templates -->

</xsl:stylesheet>
