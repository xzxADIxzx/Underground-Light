shader_type canvas_item;
render_mode unshaded;

uniform float strength : hint_range(0.0, 2.0);

void fragment() {
	float x = SCREEN_UV.x - 0.5, y = SCREEN_UV.y - 0.5;
	COLOR = vec4(0.0, 0.0, 0.0, sqrt(x * x + y * y) / strength);
}
