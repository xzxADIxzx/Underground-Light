shader_type canvas_item;
render_mode unshaded;

const float noise = 0.01;

uniform sampler2D screen : hint_screen_texture, repeat_disable, filter_nearest;

float random(vec2 uv) {
	return fract(sin(dot(uv, vec2(12.98, 78.23))) * 43758.54);
}

void fragment()
{
	float line_1 = smoothstep(0.999, 1.0, sin(UV.y * 4.0 - TIME)) * 0.02;
	float line_2 = smoothstep(0.999, 1.0, sin(UV.y * 6.0 - TIME * 1.9)) * 0.02;

	float strength = smoothstep(0.15, 0.5, 1.0 - abs(UV.y - 0.5) * 2.0);

	vec2 disto_1 = vec2(line_1 / 20.0, line_1) * strength;
	vec2 disto_2 = vec2(line_2 / 16.0, line_2) * strength;

	COLOR = texture(screen, SCREEN_UV + disto_1 + disto_2);
	COLOR.rgb += random(vec2(ceil(SCREEN_UV.y * 60.0) / 60.0 + TIME)) * noise;
}
