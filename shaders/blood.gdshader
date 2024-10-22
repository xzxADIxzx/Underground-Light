shader_type canvas_item;
render_mode unshaded;

uniform float strength : hint_range(-0.4, 1.0);

float random(vec2 uv) {
	return fract(sin(dot(uv, vec2(12.98, 78.23))) * 43758.54);
}

float worley(vec2 uv) {
	float dist = 1.0;

	for (float y = -1.0; y <= 1.0; y++) {
		for (float x = -1.0; x <= 1.0; x++) {
			// static random
			vec2 random = vec2(random(vec2(x, y) + floor(uv)));
			// dynamic random
			vec2 wobble = vec2(cos(TIME * random.x), sin(TIME * random.y));
			// distance to the (neightbor + random)
			dist = min(dist, length(vec2(x, y) - fract(uv) + random + wobble * 0.2));
		}
	}

	return dist;
}

void fragment() {
	float aspect = SCREEN_PIXEL_SIZE.x / SCREEN_PIXEL_SIZE.y;
	float worley = worley(UV * vec2(16.0, 16.0 * aspect));

	float x = SCREEN_UV.x - 0.5, y = SCREEN_UV.y - 0.5;
	float vignette = sqrt(x * x + y * y) * 2.0 + strength - 1.0;

	COLOR = vec4(worley * (vignette + 0.2), 0.0, 0.0, worley * vignette);
}
