SHELL := /bin/bash

help:
	@echo "Please use \`make <target>' where <target> is one of"
	@echo "  all               to update, generate and test this SDK"
	@echo "  update            to update git submodules"
	@echo "  generate          to generate service code"

all: update generate

generate:
	@if [[ ! -f "$$(which snips)" ]]; then \
		echo "ERROR: Command \"snips\" not found."; \
	fi
	snips \
		--service=qingstor --service-api-version=latest \
		--spec="./specs" --template="./template" --output="./Service"
	rm ./Service/Object.cs
	@echo "ok"

update:
	git submodule update --remote
	@echo "ok"