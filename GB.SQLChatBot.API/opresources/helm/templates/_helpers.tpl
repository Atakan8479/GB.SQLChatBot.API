{{/*
Expand the name of the chart.
*/}}
{{- define "gb-sqlchatbot-service.name" -}}
{{- default .Chart.Name .Values.nameOverride | trunc 63 | trimSuffix "-" }}
{{- end }}

{{/*
Common labels
*/}}
{{- define "gb-sqlchatbot-service.labels" -}}
{{ include "gb-sqlchatbot-service.selectorLabels" . }}
{{- if .Chart.AppVersion }}
app.kubernetes.io/version: {{ .Chart.AppVersion | quote }}
{{- end }}
app.kubernetes.io/managed-by: {{ .Release.Service }}
{{- end }}

{{/*
Selector labels
*/}}
{{- define "gb-sqlchatbot-service.selectorLabels" -}}
{{- range $key, $value := .Values.selectorLabels }}
{{ $key }}: {{ $value }}
{{- end }}
{{- end }}

{{/*
Environment variables
*/}}
{{- define "gb-sqlchatbot-service.envVariables" -}}
{{- range $key, $value := .Values.envVars }}
- name: {{ $value.name }}
  value: {{ $value.value }}
{{- end}}
{{- end }}

Renders a value that contains template.
Usage:
{{ include "common.tplvalues.render" ( dict "value" .Values.path.to.the.Value "context" $) }}
*/}}
{{- define "gb-sqlchatbot-service.tplvalues.render" -}}
    {{- if typeIs "string" .value }}
        {{- tpl .value .context }}
    {{- else }}
        {{- tpl (.value | toYaml) .context }}
    {{- end }}
{{- end -}}