package com.realt.statsapi.controllers;

import com.realt.statsapi.controllers.dto.*;
import com.realt.statsapi.data.HistoryItem;
import com.realt.statsapi.data.HistoryRepository;
import org.modelmapper.ModelMapper;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@Controller
@RequestMapping("history")
public class HistoryController {

    private static final Logger log = LoggerFactory.getLogger(HistoryController.class);

    private ModelMapper modelMapper = new ModelMapper();

    @Autowired
    private HistoryRepository repository;

    @PostMapping()
    public @ResponseBody HistoryAddResponse add(@RequestBody HistoryAddRequest request) {

        log.debug("History add request [{}]", request);

        // prepare
        ArrayList<HistoryItem> historyItems = new ArrayList<HistoryItem>();
        for (Property property: request.getItems()){
            HistoryItem historyItem = modelMapper.map(property, HistoryItem.class);
            historyItem.setScanId(request.getScanId());
            historyItem.setSource(request.getSource());
            historyItems.add(historyItem);
            log.info("Adding new history item: {}", historyItem);
        }

        // execute
        log.debug("Saving batch {} items...", historyItems.size());
        List<HistoryItem> result = repository.saveAll(historyItems);
        log.info("Saved batch {} items", result.size());

        // build response
        HistoryAddResponse response = new HistoryAddResponse();
        response.setAdded(result.size());
        return response;
    }

    @DeleteMapping("{source}/{scanId}")
    public @ResponseBody HistoryClearResponse deleteV2(@PathVariable int source, @PathVariable String scanId) {
        HistoryClearRequest request = new HistoryClearRequest();
        request.setSource(source);
        request.setScanId(scanId);
        return delete(request);
    }

    @DeleteMapping()
    public @ResponseBody HistoryClearResponse delete(@RequestBody HistoryClearRequest request) {

        log.info("History cleanup request [{}]", request);

        // execute
        Long deleted = repository.deleteByScanIdAndSource(request.getScanId(), request.getSource());

        log.info("History cleanup done, removed = {}", deleted);

        // build response
        HistoryClearResponse response = new HistoryClearResponse();
        response.setRemoved(deleted.intValue());
        return response;
    }
}