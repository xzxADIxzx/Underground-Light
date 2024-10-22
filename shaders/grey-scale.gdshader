shader_type canvas_item;
render_mode unshaded;

const vec3 sum = vec3(1.0 / 3.0);

uniform float strength : hint_range(0.0, 1.0);
uniform sampler2D screen : hint_screen_texture, repeat_disable, filter_nearest;

void fragment() {
	vec3 col = texture(screen, SCREEN_UV).rgb;
	COLOR.rgb = mix(col, vec3(dot(col, sum)), strength);
}
