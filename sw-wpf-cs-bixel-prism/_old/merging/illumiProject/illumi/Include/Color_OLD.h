#pragma once

#include <iostream>
#include <Windows.h>


struct ColorHSV;
struct ColorRGB;


struct ColorHSV
{
	byte hue, saturation, value;

	ColorHSV()
	{
		hue = saturation = value = 0;
	}

	ColorHSV(byte h, byte s, byte v)
	{
		hue = h;
		saturation = s;
		value = v;
	}

	//ColorHSV fromRGB(ColorRGB rgb)
	//{
	//	byte min, max;

	//	min = min(rgb.red, min(rgb.green, rgb.blue));
	//	max = max(rgb.red, max(rgb.green, rgb.blue));

	//	value = max;
	//	if (value == 0)
	//	{
	//		hue = 0;
	//		saturation = 0;
	//	}

	//	saturation = (max - min) / max;
	//	if (saturation == 0)
	//	{
	//		hue = 0;
	//	}

	//	if (rgb.red == max)
	//	{
	//		hue = 0 + 43 * (rgb.green - rgb.blue) / (max - min);
	//	}
	//	else if (rgb.green == max)
	//	{
	//		hue = 67 + 43 * (rgb.blue - rgb.red) / (max - min);
	//	}
	//	else
	//	{
	//		hue = 127 + 43 * (rgb.red - rgb.green) / (max - min);
	//	}
	//}
};

struct ColorRGB
{
	byte red, green, blue;

	ColorRGB()
	{
		red = green = blue = 0;
	}

	ColorRGB(byte r, byte g, byte b)
	{
		red = r;
		green = g;
		blue = b;
	}

	ColorRGB fromHSV(ColorHSV hsv)
	{
		unsigned char region, p, q, t;
		unsigned int remainder;

		if (hsv.value == 0)
		{
			red = green = blue = 0;
		}

		if (hsv.saturation == 0)
		{
			red = green = blue = hsv.value;
		}

		region = hsv.hue / 43;
		remainder = (hsv.hue - (region * 43)) * 6;

		p = (hsv.value * (255 - hsv.saturation)) >> 8;
		q = (hsv.value * (255 - ((hsv.saturation * remainder) >> 8))) >> 8;
		t = (hsv.value * (255 - ((hsv.saturation * (255 - remainder)) >> 8))) >> 8;

		switch (region)
		{
		case 0:
			red = hsv.value;
			green = t;
			blue = p;
			break;
		case 1:
			red = q;
			green = hsv.value;
			blue = p;
			break;
		case 2:
			red = p;
			green = hsv.value;
			blue = t;
			break;
		case 3:
			red = p;
			green = q;
			blue = hsv.value;
			break;
		case 4:
			red = t;
			green = p;
			blue = hsv.value;
			break;
		default:
			red = hsv.value;
			green = p;
			blue = q;
			break;
		}
	}
};
