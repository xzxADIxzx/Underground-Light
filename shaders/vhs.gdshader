shader_type canvas_item;
render_mode unshaded;

const float brightness = 1.3;
const float saturation = 0.4;
const float contrast = 1.2;
const float noise = 0.04;

const vec3 sum = vec3(1.0 / 3.0);
const vec2 res = vec2(160.0, 120.0);

uniform sampler2D screen : hint_screen_texture, repeat_disable, filter_nearest;

float random(vec2 uv) {
	return fract(sin(dot(uv, vec2(12.98, 78.23))) * 43758.54);
}

void fragment()
{
	vec2 cuv = ceil(SCREEN_UV * res) / res;
	vec2 puv = SCREEN_UV * PI * res;

	vec4 col = texture(screen, cuv);

	// grille
	float r = smoothstep(0.88, 0.92, abs(sin(PI * 1.0 / 3.0 + puv.x)));
	col.r = mix(col.r, col.r * r, 0.3);

	float g = smoothstep(0.88, 0.92, abs(sin(PI * 0.0 / 3.0 + puv.x)));
	col.g = mix(col.g, col.g * g, 0.3);

	float b = smoothstep(0.88, 0.92, abs(sin(PI * 2.0 / 3.0 + puv.x)));
	col.b = mix(col.b, col.b * b, 0.3);

	float h = smoothstep(0.25, 0.45, abs(sin(puv.y)));
	col.rgb = mix(col.rgb, col.rgb * h, 0.4);

	// brightness
	col = clamp(col * brightness, 0.0, 1.0);

	// saturation
	vec3 greyscale = vec3(dot(col.rgb, sum));
	col.rgb = mix(col.rgb, greyscale, saturation);

	// contrast
	float midpoint = pow(0.5, 2.2);
	col.rgb = (col.rgb - vec3(midpoint)) * contrast + midpoint;

	// noise
	col.rgb += random(cuv + TIME) * noise;

	COLOR = col;
}
