#pragma once


struct colorRGB
{
	byte red, green, blue, alpha;

	colorRGB();
	colorRGB(byte r, byte g, byte b, byte a);

	void Set(byte r, byte g, byte b, byte a);

	void operator =(const colorRGB &in);
};

colorRGB lerp(colorRGB min, colorRGB max, float fac);
