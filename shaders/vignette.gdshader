shader_type canvas_item;
render_mode unshaded;

uniform mat4 dithering;
uniform float strength : hint_range(0.0, 2.0);

void fragment() {
	float x = SCREEN_UV.x - 0.5, y = SCREEN_UV.y - 0.5;
	float d = dithering
			[int(UV.x / SCREEN_PIXEL_SIZE.x / 2.0) % 4]
			[int(UV.y / SCREEN_PIXEL_SIZE.y / 2.0) % 4];

	COLOR = vec4(0.0, 0.0, 0.0, sqrt(x * x + y * y) / strength + d * 0.001);
}
