<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:param name="name" select="''" />
	<xsl:param name="faculty" select="''" />
	<xsl:param name="buildnum" select="''" />
	<xsl:param name="roomnum" select="''" />
	<xsl:param name="restime" select="''" />
	<xsl:param name="docnum" select="''" />

	<xsl:template match="/">
		<html>
			<head>
				<style>
					table { border-collapse: collapse; width: 100%; }
					th, td { border: 1px solid black; padding: 8px; text-align: left; }
					th { background-color: #f2f2f2; }
				</style>
			</head>
			<body>
				<table>
					<tr>
						<th>Name</th>
						<th>Faculty</th>
						<th>BuildNum</th>
						<th>RoomNum</th>
						<th>ResTime</th>
						<th>DocNum</th>
					</tr>
					<xsl:for-each select="//student[
                        ($name='' or @NAME=$name) and
                        ($faculty='' or @FACULTY=$faculty) and
                        ($buildnum='' or @BUILDNUM=$buildnum) and
                        ($roomnum='' or @ROOMNUM=$roomnum) and
                        ($restime='' or @RESTIME=$restime) and
                        ($docnum='' or @DOCNUM=$docnum)
                    ]">
						<tr>
							<td>
								<xsl:value-of select="@NAME"/>
							</td>
							<td>
								<xsl:value-of select="@FACULTY"/>
							</td>
							<td>
								<xsl:value-of select="@BUILDNUM"/>
							</td>
							<td>
								<xsl:value-of select="@ROOMNUM"/>
							</td>
							<td>
								<xsl:value-of select="@RESTIME"/>
							</td>
							<td>
								<xsl:value-of select="@DOCNUM"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
