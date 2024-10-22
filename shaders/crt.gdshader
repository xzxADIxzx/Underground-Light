shader_type canvas_item;
render_mode unshaded;

uniform sampler2D screen : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
	vec2 shift = (vec2(2.0) - sin(UV.yx * PI)) * 0.05;
	vec2 scale = vec2(1.0) + shift * 2.0;

	vec2 uv = UV * scale - shift;
	bool mask = uv.x >= 0.0 && uv.x <= 1.0 && uv.y >= 0.0 && uv.y <= 1.0;

	COLOR = mask ? texture(screen, uv) : vec4(0.0, 0.0, 0.0, 1.0);
	
	if (mask)
	{
		float x = uv.x - 0.5, y = uv.y - 0.5;
		COLOR.rgb *= 1.0 - sqrt(x * x + y * y) * 1.1;
	}
}
