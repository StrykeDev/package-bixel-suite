
#include "pch.h"

#include "Colors.h"


colorRGB::colorRGB()
{
	red = 0;
	green = 0;
	blue = 0;
	alpha = 255;
}

colorRGB::colorRGB(byte r, byte g, byte b, byte a)
{
	red = r;
	green = g;
	blue = b;
	alpha = a;
}

void colorRGB::Set(byte r, byte g, byte b, byte a)
{
	red = r;
	green = g;
	blue = b;
	alpha = a;
}

void colorRGB::operator=(const colorRGB & in)
{
	red = in.red;
	green = in.green;
	blue = in.blue;
	alpha = in.alpha;
}

colorRGB lerp(colorRGB min, colorRGB max, float fac)
{
	if (fac > 1)
	{
		fac = 1;
	}
	else if (fac < 0)
	{
		fac = 0;
	}

	colorRGB out;

	out.red = (byte)(((float)max.red - min.red) * fac + min.red);
	out.green = (byte)(((float)max.green - min.green) * fac + min.green);
	out.blue = (byte)(((float)max.blue - min.blue) * fac + min.blue);
	out.alpha = (byte)(((float)max.alpha - min.alpha) * fac + min.alpha);

	return out;
}
