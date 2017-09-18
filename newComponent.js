const fs = require('fs');
const path = require('path');

function newComponent(componentName, savePath) {
    console.log('saving new component', componentName, savePath)
    let content = componentText(componentName);
    let filePath = path.join(__dirname, savePath, `${componentName}.tsx`)
    for (var i = 0; i < content.length; i++) {
        try {
        fs.appendFileSync(filePath, content[i] + '\n');
        } catch (e) {
            console.error(e);
            process.exit(0);
        }
    }
}

function componentText(name) {
    let stateName = `I${name}State`;
    let propsName = `I${name}Props`;
    return [
        `import * as React from 'react';`,
        '',
        interface(stateName),
        '',
        interface(propsName),
        '',
        `export default class ${name} extends React.Component<${propsName}, ${stateName}> {`,
        '',
        `    render() {`,
        `        return (`,
        `            <div/>`,
        `        );`,
        `    }`,
        '}'
    ]
}

function interface(name) {
    return `interface ${name} {\n\n}`;
}

let componentName = process.argv[2];
let folder = process.argv[3];
if (!componentName || !folder) {
    console.log('usage `node ./newComponent.js [component name] [folder]');
    process.exit(1);
}

newComponent(componentName, folder);