docker build -t realt.parser.onliner -f ./realt.parser.onliner.dockerfile .
docker build -t realt.parser.realt -f ./realt.parser.realt.dockerfile .
docker build -t realt.ui -f ./realt.ui.dockerfile .
docker build -t realt.stats.api -f ./realt.stats.api.dockerfile
