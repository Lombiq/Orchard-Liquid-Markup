﻿
namespace Lombiq.LiquidMarkup
{
    public static class Constants
    {
        /// <summary>
        /// When shapes are displayed recursively (i.e. a shape displays itself) below this depth display will be prevented
        /// to avoid stack overflow. Recursion can be reasonable to a degree.
        /// </summary>
        public const int MaxAllowedShapeRecursionDepth = 3;

        /// <summary>
        /// When a shape object is displayed multiple times it can be legitimate (although rare) but can also indicate 
        /// unwanted recursion. This configuration is a cap for the display count.
        /// </summary>
        public const int MaxAllowedShapeDisplayCount = 10;

        /// <summary>
        /// Key for the Liquid context value storing the <see cref="Models.TemplateRenderingContext"/> object.
        /// </summary>
        public const string TemplateRenderingContextKey = "TemplateRenderingContext";
    }
}