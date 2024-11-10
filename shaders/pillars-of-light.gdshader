shader_type spatial;

uniform vec3 color : source_color;
uniform float strength : hint_range(0.0, 16.0);

void vertex() {
	float modifier = VERTEX.x > 0.0 ? 1.0 : -1.0;

	vec3 center = vec3(0.13 * modifier, VERTEX.y, 0.0);
	vec3 normal = VERTEX - center;

	float base   =                       sin(TIME * modifier * 16.0 + VERTEX.y *  6.0)  * 0.06;
	float line_1 = smoothstep(0.99, 1.0, sin(TIME * modifier *  4.0 + VERTEX.y *  4.0)) * 0.4;
	float line_2 = smoothstep(0.88, 1.0, sin(TIME * modifier * 16.0 + VERTEX.y * 32.0)) * 0.1;
	float line_3 = smoothstep(0.11, 1.0, sin(TIME * modifier *-24.0 + VERTEX.y * 64.0)) * 0.1;

	VERTEX += normal * (base + line_1 + line_2 + line_3);
}

void fragment() {
	EMISSION = color * strength;
}
