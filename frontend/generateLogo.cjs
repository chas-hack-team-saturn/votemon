const fs = require("fs");
const path = require("path");

let svg = fs.readFileSync(
  path.join(__dirname, "src/assets/vtmnlogo_light_landscape.svg"),
  "utf8",
);

// remove <?xml ... ?>
svg = svg.replace(/<\?xml.*?\?>/g, "");

// camelCase attributes
svg = svg.replace(/clip-path/g, "clipPath");
svg = svg.replace(/stroke-width/g, "strokeWidth");
svg = svg.replace(/stop-color/g, "stopColor");
svg = svg.replace(/xmlns:xlink/g, "xmlnsXlink");

// replace style strings with objects
svg = svg.replace(/style="([^"]*)"/g, (match, p1) => {
  const styleObj = {};
  p1.split(";").forEach((rule) => {
    if (!rule.trim()) return;
    let [key, value] = rule.split(":");
    key = key.trim().replace(/-([a-z])/g, (g) => g[1].toUpperCase());
    styleObj[key] = value.trim();
  });

  // manual string building to allow injecting the textColor variable
  let styleProps = [];
  for (const [k, v] of Object.entries(styleObj)) {
    if (k === "fill" && v === "#39292f") {
      styleProps.push(`${k}: textColor`);
    } else if (k === "strokeWidth" && v === "0px") {
      styleProps.push(`${k}: 0`);
    } else {
      styleProps.push(`${k}: '${v}'`);
    }
  }
  return `style={{ ${styleProps.join(", ")} }}`;
});

// add the component wrapper
const component = `
import { useTheme } from '../context/ThemeContext';

export default function Logo({ className }: { className?: string }) {
    const { theme } = useTheme();
    const textColor = theme === 'light' ? '#39292f' : '#fff';

    return (
        ${svg.replace("<svg ", "<svg className={className} ")}
    );
}
`;

fs.writeFileSync(
  path.join(__dirname, "src/components/Logo.tsx"),
  component.trim(),
);
console.log("Logo.tsx generated!");
